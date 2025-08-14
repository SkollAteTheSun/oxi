using Common.HttpClientWrapper;
using Common.RabbitMQClient;
using Kp.Ms.Oxi.Config;
using Kp.Ms.Oxi.Context;
using Kp.Ms.Oxi.Extensions;
using Kp.Ms.Oxi.Models.Entity;
using Kp.Ms.Oxi.Models.Enums;
using Kp.Ms.Oxi.Models.Opera;
using Kp.Ms.Oxi.Models.Request;
using Kp.Ms.Oxi.Models.Response;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;
using Npgsql;
using OpenSearch.Client;
using System.Net.Mime;
using System.Text;
using System.Text.RegularExpressions;

namespace Kp.Ms.Oxi.Services;

public class PmsService
{
    private readonly ILogger<PmsService> _logger;
    private readonly IRabbitMQClient _rabbitMQClient;
    private readonly IHttpClientWrapper _httpClient;
    private readonly OpenSearchClient _openSearchClient;
    private readonly IDbContextFactory<BookingContext> _bookingContextFactory;
    private readonly CheckOutProcessingSettings _settings;
    private readonly IDbContextFactory<DefaultContext> _contextFactory;

    private IDictionary<string, Hotel> _hotels = new Dictionary<string, Hotel>();

    private const string QUEUE_PREFIX = "check-out-queue";
    private const string EXCHANGE_PREFIX = "check-out-exchange";
    private const string ROUTING_PREFIX = "check-out-routing";

    private readonly IDictionary<string, string> _mindboxDefaultHeaders;
    private readonly IDictionary<string, string> _bookingDefaultHeaders;
    private const string _authScheme = "Bearer";
    private const string _uniqueViolation = "23505";

    private readonly Regex _bookingIdRegex = new Regex("^[0-9]+-[0-9]+-[0-9]+$");

    public PmsService(
        ILogger<PmsService> logger,
        IRabbitMQClient rabbitMQClient,
        IHttpClientWrapper httpClient,
        OpenSearchClient openSearchClient,
        IDbContextFactory<BookingContext> bookingContextFactory,
        IOptions<CheckOutProcessingSettings> settings,
        IDbContextFactory<DefaultContext> contextFactory)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _rabbitMQClient = rabbitMQClient ?? throw new ArgumentNullException(nameof(rabbitMQClient));
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        _openSearchClient = openSearchClient ?? throw new ArgumentNullException(nameof(openSearchClient));
        _bookingContextFactory = bookingContextFactory ?? throw new ArgumentNullException(nameof(bookingContextFactory));
        _settings = settings.Value ?? throw new ArgumentNullException(nameof(settings));
        _contextFactory = contextFactory ?? throw new ArgumentNullException(nameof(contextFactory));

        _mindboxDefaultHeaders = new Dictionary<string, string>()
        {
            { HeaderNames.Authorization, $"{_authScheme} {_settings.Mindbox.Token}" }
        };

        _bookingDefaultHeaders = new Dictionary<string, string>()
        {
            { "api_token", _settings.Booking.ApiToken }
        };

        using (var context = _contextFactory.CreateDbContext())
        {
            _hotels = context
                .Hotels
                .ToDictionary(key => key.Code, value => value);
        }
        
        CreateProcessingChannelsForCheckOuts(_hotels.Keys.ToList());
    }

    public IEnumerable<Hotel> GetHotels()
    {
        return _hotels.Values.ToList();
    }

    public async Task<Hotel> AddHotel(HotelContract hotel)
    {
        if (_hotels.TryGetValue(hotel.Code, out var existsHotel))
        {
            return existsHotel;
        }
        else
        {
            var newHotel = await SaveHotel(new Hotel()
            {
                Name = hotel.Name,
                Code = hotel.Code
            });
            _hotels.Add(newHotel.Code, newHotel);

            CreateProcessingChannelsForCheckOuts(new List<string>() { newHotel.Code });

            return newHotel;
        }
    }

    public async Task<IEnumerable<CheckOutResult>> AddCheckOutTasksAsync(IEnumerable<CheckOutInfo> checkOuts)
    {
        var resultList = new List<CheckOutResult>();

        foreach (var checkOut in checkOuts)
        {
            var result = new CheckOutResult()
            {
                HotelCode = checkOut.HotelCode,
                ExternalId = checkOut.ExternalId
            };

            try
            {
                await LogCheckOut(new CheckOut(checkOut), CheckOutProcessingStatus.Detected);

                if (!_hotels.ContainsKey(checkOut.HotelCode))
                {
                    throw new Exception($"The hotel with code \"{checkOut.HotelCode}\" not found.");
                }

                _rabbitMQClient.SendMessage(
                    $"{EXCHANGE_PREFIX}-{checkOut.HotelCode}",
                    $"{QUEUE_PREFIX}-{checkOut.HotelCode}",
                    $"{ROUTING_PREFIX}-{checkOut.HotelCode}",
                    checkOut,
                    true);

                await LogCheckOut(new CheckOut(checkOut), CheckOutProcessingStatus.Queued);

                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.ErrorMessage = ex.Message;
            }

            resultList.Add(result);
        }

        return resultList;
    }

    public async Task ResendCheckOuts()
    {
        List<CheckOut> unprocessedCheckOuts = [];
        using (var context = await _contextFactory.CreateDbContextAsync())
        {
            unprocessedCheckOuts = await context
                .CheckOuts
                .AsNoTracking()
                .Where(c => !c.Processed)
                .ToListAsync();
        }

        foreach (var checkOut in unprocessedCheckOuts)
        {
            try
            {
                await RunCheckOutPipLine(checkOut);
            }
            catch (Exception ex)
            {
                var errorMessage = $"An error occurred while resend the check-out. {ex.Message}";
                _logger.LogError(errorMessage);

                await LogCheckOut(checkOut, CheckOutProcessingStatus.Failed, errorMessage);
            }
        }
    }

    private async Task<Hotel> SaveHotel(Hotel hotel)
    {
        using var context = await _contextFactory.CreateDbContextAsync();

        var hotelEntity = await context
            .Hotels
            .AddAsync(hotel);
        await context.SaveChangesAsync();

        return hotelEntity.Entity;
    }

    private void CreateProcessingChannelsForCheckOuts(List<string> hotels)
    {
        hotels?.ForEach(id =>
        {
            _rabbitMQClient.CreateQueue($"{EXCHANGE_PREFIX}-{id}", $"{QUEUE_PREFIX}-{id}", $"{ROUTING_PREFIX}-{id}");
            _rabbitMQClient.CreateConsumer($"{EXCHANGE_PREFIX}-{id}", $"{QUEUE_PREFIX}-{id}", $"{ROUTING_PREFIX}-{id}", async (model, arg) =>
            {
                CheckOutInfo checkOut = null;

                try
                {
                    try
                    {
                        var body = arg.Body.ToArray();
                        checkOut = JsonConvert.DeserializeObject<CheckOutInfo>(Encoding.UTF8.GetString(body));
                    }
                    catch
                    {
                        _logger.LogWarning("Cannot parsing check-out.");
                        return;
                    }

                    if (checkOut == null)
                    {
                        _logger.LogWarning("Check-out is empty.");
                        return;
                    }

                    if (_settings.IgnoreHotels.Contains(checkOut?.HotelCode))
                    {
                        _logger.LogWarning($"The hotel \"{checkOut?.HotelCode}\" was ignored.");
                        return;
                    }

                    var checkOutEntity = await SaveCheckOut(checkOut);
                    if (checkOutEntity.Processed)
                    {
                        _logger.LogWarning($"The check-out \"{checkOutEntity.HotelCode}:{checkOutEntity.PmsId}\" already processed.");
                        return;
                    }

                    await RunCheckOutPipLine(checkOutEntity);
                }
                catch (Exception ex)
                {
                    var errorMessage = $"An error occurred while processing the check-out. {ex.Message}";
                    _logger.LogError(errorMessage);

                    await LogCheckOut(new CheckOut(checkOut), CheckOutProcessingStatus.Failed, errorMessage);

                    await SendErrorToMail(checkOut, errorMessage);

                    throw ex;
                }
            });
        });
    }

    private async Task RunCheckOutPipLine(CheckOut checkOutEntity)
    {
        if (string.IsNullOrEmpty(checkOutEntity.BookingId) || !_bookingIdRegex.IsMatch(checkOutEntity.BookingId))
        {
            _logger.LogWarning($"The booking id \"{checkOutEntity.BookingId}\" does not match the format.");
            await SetCheckOutProcessed(checkOutEntity);
            return;
        }

        var bookingReservation = await GetReservationByNumber(checkOutEntity.BookingId);
        if (bookingReservation == null)
        {
            _logger.LogWarning($"The reservation for booking id \"{checkOutEntity.BookingId}\" not found.");
            await SetCheckOutProcessed(checkOutEntity);
            return;
        }

        var statuses = await GetCheckOutStatuses(checkOutEntity.BookingId);

        if (!statuses.TryGetValue(CheckOutProcessingStatus.SendedToBooking, out _))
        {
            await SendToBooking(checkOutEntity, checkOutEntity.BookingId, bookingReservation.Id.ToString(), checkOutEntity.CheckOutDate);
        }

        if (!statuses.TryGetValue(CheckOutProcessingStatus.ReservationStatusEdited, out _))
        {
            await EditReservationStatusInShop(checkOutEntity, checkOutEntity.BookingId, bookingReservation.Id.ToString());
        }

        if (!statuses.TryGetValue(CheckOutProcessingStatus.FullPaymentReceiptSended, out _))
        {
            await SendFullPaymentReceiptInShop(checkOutEntity, checkOutEntity.BookingId, bookingReservation.OrderNumber);
        }

        if (!statuses.TryGetValue(CheckOutProcessingStatus.SendedToMindbox, out _))
        {
            await SendToMindbox(checkOutEntity, checkOutEntity.BookingId);
        }

        if (!statuses.TryGetValue(CheckOutProcessingStatus.SendingToMindboxDateSet, out _))
        {
            await SetSendingToMindboxDate(checkOutEntity, bookingReservation);
        }

        await SetCheckOutProcessed(checkOutEntity);
    }

    private async Task<CheckOut> SaveCheckOut(CheckOutInfo checkOut)
    {
        _logger.LogInformation($"Try save check-out \"{checkOut.HotelCode}:{checkOut.Id}\" to DB.");

        var existsCheckOut = await GetCheckOut(checkOut);
        if (existsCheckOut != null)
        {
            _logger.LogWarning($"The check-out \"{checkOut.HotelCode}:{checkOut.Id}\" already exists.");
            await LogCheckOut(existsCheckOut, CheckOutProcessingStatus.Saved);
            return existsCheckOut;
        }

        using var context = await _contextFactory.CreateDbContextAsync();

        try
        {
            var checkoutEntity = new CheckOut()
            {
                PmsId = checkOut.Id,
                ExternalId = checkOut.ExternalId,
                BookingId = checkOut.BookingId,
                CheckOutDate = checkOut.CheckOutDate,
                HotelCode = checkOut.HotelCode
            };

            var result = await context
                .CheckOuts
                .AddAsync(checkoutEntity);

            await context.SaveChangesAsync();
            await LogCheckOut(result.Entity, CheckOutProcessingStatus.Saved);

            _logger.LogInformation($"The check-out \"{checkOut.HotelCode}:{checkOut.Id}\" saved to DB successfully.");

            return result.Entity;
        }
        catch (Exception ex)
        {
            await context.DisposeAsync();
            throw ex;
        }
    }

    private async Task<CheckOut?> GetCheckOut(CheckOutInfo checkOut)
    {
        using var context = await _contextFactory.CreateDbContextAsync();
        
        var existsCheckOut = await context
            .CheckOuts
            .FirstOrDefaultAsync(c => c.HotelCode == checkOut.HotelCode && c.PmsId == checkOut.Id);

        return existsCheckOut;
    }

    private async Task<HashSet<CheckOutProcessingStatus>> GetCheckOutStatuses(string bookingId)
    {
        _logger.LogInformation($"Try get check-out statuses for reservation \"{bookingId}\"...");

        using var context = await _contextFactory.CreateDbContextAsync();

        var checkOutStatuses = context
            .CheckOutStatuses
            .AsNoTracking()
            .Where(c => c.BookingId == bookingId)
            .Select(c => c.Status)
            .ToHashSet();

        _logger.LogInformation($"Check-out statuses for reservation \"{bookingId}\" got successfully.");

        return checkOutStatuses;
    }

    private async Task SetCheckOutProcessed(CheckOut checkOut)
    {
        _logger.LogInformation($"Try set check-out processing status for reservation \"{checkOut.BookingId}\"...");

        using var context = await _contextFactory.CreateDbContextAsync();

        checkOut.Processed = true;
        var checkOutStatuses = context
            .CheckOuts
            .Update(checkOut);

        await context.SaveChangesAsync();

        _logger.LogInformation($"Check-out processing status for reservation \"{checkOut.BookingId}\" set successfully.");
    }

    private async Task AddCheckOutStatus(string bookingId, CheckOutProcessingStatus status)
    {
        _logger.LogInformation($"Try add check-out status \"{status}\" for reservation \"{bookingId}\"...");

        using var context = await _contextFactory.CreateDbContextAsync();

        var checkOutStatus = new CheckOutStatus()
        {
            Id = Guid.NewGuid(),
            BookingId = bookingId,
            Status = status,
            Date = DateTime.UtcNow
        };

        var checkOutStatuses = await context
            .CheckOutStatuses
            .AddAsync(checkOutStatus);

        await context.SaveChangesAsync();

        _logger.LogInformation($"Check-out status \"{status}\" for reservation \"{bookingId}\" added successfully.");
    }

    public async Task SendToBooking(CheckOut? checkOut, string reservationNumber, string reservationId, DateTime checkOutDate)
    {
        try
        {
            if (!_settings.Booking.Enabled)
            {
                return;
            }

            _logger.LogInformation($"Try send data by reservation \"{reservationId}\" to Booking...");

            if (string.IsNullOrWhiteSpace(reservationId))
            {
                _logger.LogWarning($"Invalid booking id for reservation: {reservationId}");
                return;
            }

            var contract = new BookingContract()
            {
                ReservationId = reservationId,
                DateCheckOut = checkOutDate,
                ApiToken = _settings.Booking.ApiToken
            };

            var result = await _httpClient.PostAsync<BookingContract, object>(
                _settings.Booking.Url,
                contract,
                headers: _bookingDefaultHeaders);

            await AddCheckOutStatus(reservationNumber, CheckOutProcessingStatus.SendedToBooking);

            if (checkOut != null)
            {
                await LogCheckOut(checkOut, CheckOutProcessingStatus.SendedToBooking);
            }

            _logger.LogInformation($"Data by reservation \"{reservationId}\" sent to Booking successfully.");
        }
        catch (Exception ex)
        {
            throw new Exception($"An error occurred while sendind check-out to Booking. {ex.Message}");
        }
    }

    public async Task SetSendingToMindboxDate(CheckOut? checkOut, BookingReservation reservation)
    {
        try
        {
            if (!_settings.Mindbox.Enabled && !_settings.Booking.Enabled)
            {
                return;
            }

            _logger.LogInformation($"Try set sending to mindbox date for reservation \"{reservation.Id}\"...");

            reservation.MindboxPointsDate = DateTime.UtcNow;

            using var context = await _bookingContextFactory.CreateDbContextAsync();
            context.Reservations.Attach(reservation);
            context.Entry(reservation).Property(x => x.MindboxPointsDate).IsModified = true;
            await context.SaveChangesAsync();

            await AddCheckOutStatus(reservation.Number, CheckOutProcessingStatus.SendingToMindboxDateSet);

            if (checkOut != null)
            {
                await LogCheckOut(checkOut, CheckOutProcessingStatus.SendingToMindboxDateSet);
            }

            _logger.LogInformation($"Sending to mindbox date for reservation \"{reservation.Id}\" set successfully.");
        }
        catch (Exception ex)
        {
            throw new Exception($"An error occurred while setting sendind to mindbox date for reservation. {ex.Message}");
        }
    }

    public async Task EditReservationStatusInShop(CheckOut? checkOut, string reservationNumber, string reservationId)
    {
        try
        {
            if (!_settings.ShopEditReservationStatus.Enabled)
            {
                return;
            }

            _logger.LogInformation($"Try edit status for reservation \"{reservationId}\" in Shop...");

            if (string.IsNullOrWhiteSpace(reservationId))
            {
                _logger.LogWarning($"Invalid reservation id: {reservationId}");
                return;
            }

            var reservationContract = new ShopReservationContract()
            {
                Key = _settings.ShopEditReservationStatus.ApiKey,
                Number = reservationNumber,
                Id = reservationId,
                Status = ShopReservationStatus.finished.ToString()
            };

            var reservationResult = await _httpClient.PostAsync<ShopReservationContract, object>(_settings.ShopEditReservationStatus.Url, reservationContract);
            
            await AddCheckOutStatus(reservationNumber, CheckOutProcessingStatus.ReservationStatusEdited);

            if (checkOut != null)
            {
                await LogCheckOut(checkOut, CheckOutProcessingStatus.ReservationStatusEdited);
            }

            _logger.LogInformation($"Status for reservation \"{reservationId}\" in Shop edited successfully.");
        }
        catch (Exception ex)
        {
            throw new Exception($"An error occurred while editing reservation status by check-out in Shop. {ex.Message}");
        }
    }

    public async Task SendFullPaymentReceiptInShop(CheckOut? checkOut, string reservationNumber, string orderId)
    {
        try
        {
            if (!_settings.ShopFullPaymentReceipt.Enabled)
            {
                return;
            }

            _logger.LogInformation($"Try send data by reservation order id \"{orderId}\" to Shop...");

            if (string.IsNullOrWhiteSpace(orderId))
            {
                _logger.LogWarning($"Invalid order id for reservation: {orderId}");
                return;
            }

            var paymentReceiptContract = new ShopPaymentReceiptContract()
            {
                OrderId = orderId,
                Secret = _settings.ShopFullPaymentReceipt.Secret,
            };

            var paymentReceiptResult = await _httpClient.PostAsync<ShopPaymentReceiptContract, object>(
                _settings.ShopFullPaymentReceipt.Url,
                paymentReceiptContract,
                MediaTypeNames.Application.FormUrlEncoded);

            await AddCheckOutStatus(reservationNumber, CheckOutProcessingStatus.FullPaymentReceiptSended);

            if (checkOut != null)
            {
                await LogCheckOut(checkOut, CheckOutProcessingStatus.FullPaymentReceiptSended);
            }

            _logger.LogInformation($"Data by reservation order id \"{orderId}\" sent to Shop successfully.");
        }
        catch (Exception ex)
        {
            throw new Exception($"An error occurred while sendind check-out to Shop. {ex.Message}");
        }
    }

    public async Task SendToMindbox(CheckOut? checkOut, string reservationNumber)
    {
        try
        {
            if (!_settings.Mindbox.Enabled)
            {
                return;
            }

            _logger.LogInformation($"Try send data by reservation \"{reservationNumber}\" to Mindbox...");

            if (string.IsNullOrWhiteSpace(reservationNumber))
            {
                _logger.LogWarning($"Invalid booking id for reservation: {reservationNumber}");
                return;
            }

            var contract = new MindboxContract()
            {
                OrderId = reservationNumber,
                Status = 5,
                Source = _settings.Mindbox.Source
            };

            var result = await _httpClient.PostAsync<MindboxContract, MindboxResponse>(
                _settings.Mindbox.Url,
                contract,
                headers: _mindboxDefaultHeaders);

            if (!result.Success)
            {
                var splitedReservationNumber = reservationNumber.Split("-");

                if (splitedReservationNumber.Length == 3)
                {
                    contract = new MindboxContract
                    {
                        OrderId = splitedReservationNumber[1],
                        Status = 5,
                        Source = _settings.Mindbox.Source
                    };
                
                    result = await _httpClient.PostAsync<MindboxContract, MindboxResponse>(
                        _settings.Mindbox.Url,
                        contract,
                        headers: _mindboxDefaultHeaders);
                }
                
                if (!result.Success)
                {
                    throw new Exception($"{result?.ErrorMessage}. {JsonConvert.SerializeObject(result?.MindboxMessage)}");
                }
            }

            await AddCheckOutStatus(reservationNumber, CheckOutProcessingStatus.SendedToMindbox);

            if (checkOut != null)
            {
                await LogCheckOut(checkOut, CheckOutProcessingStatus.SendedToMindbox);
            }

            _logger.LogInformation($"Data by reservation \"{reservationNumber}\" sent to Mindbox successfully.");
        }
        catch (Exception ex)
        {
            throw new Exception($"An error occurred while sendind check-out to Mindbox. {ex.Message}");
        }
    }

    public async Task<BookingReservation?> GetReservationByNumber(string number)
    {
        _logger.LogInformation($"Try get booking reservation with number ${number}.");

        using var context = _bookingContextFactory.CreateDbContext();
        var reservation = await context.Reservations
            .FirstOrDefaultAsync(reservation => reservation.Number == number);

        if (reservation == null)
        {
            _logger.LogInformation($"The booking reservation with number ${number} not found.");
        }
        else
        {
            _logger.LogInformation($"The booking reservation with number ${number} got.");
        }

        return reservation;
    }

    private async Task SendErrorToMail(CheckOutInfo checkOut, string error)
    {
        try
        {
            foreach (var email in _settings.Mail.Emails)
            {
                var mail = new Mail()
                {
                    ToEmail = email,
                    Parameters = new MailParameters
                    {
                        Content = $"<h3>{error}</h3><p>{JsonConvert.SerializeObject(checkOut)}</p>"
                    }
                };

                var result = await _httpClient.PostAsync<Mail, object>(_settings.Mail.Url, mail);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError($"An error occurred while sendind error to mail. {ex.Message}");
        }
    }

    private async Task LogCheckOut(CheckOut checkOut, CheckOutProcessingStatus status, string description = "")
    {
        try
        {
            var result = await _openSearchClient.IndexAsync(new CheckOutLog()
            {
                PmsId = checkOut.PmsId,
                ExternalId = checkOut.ExternalId,
                BookingId = checkOut.BookingId,
                CheckOutDate = checkOut.CheckOutDate,
                HotelCode = checkOut.HotelCode,
                Status = status.ToString(),
                Description = description
            }, idx => idx.Index(_settings.LogIndex));
        }
        catch (Exception ex)
        {
            _logger.LogError($"An error occurred while writing log to OpenSearch. {ex.Message}");
        }
    }
}
