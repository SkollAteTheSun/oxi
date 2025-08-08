using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Serialization;
using Kp.Ms.Oxi.Context;
using Kp.Ms.Oxi.Models.Entity;
using Kp.Ms.Oxi.Models.Enums;
using Kp.Ms.Oxi.Models.Fidelio;
using Kp.Ms.Oxi.Models.Fidelio.Response;
using Kp.Ms.Oxi.Models.Opera;
using Kp.Ms.Oxi.Models.Options;
using Kp.Ms.Oxi.Models.Request;
using Kp.Ms.Oxi.Models.Request.Fidelio.Result;
using Kp.Ms.Oxi.Models.Response;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;
using LogLevel = Microsoft.Extensions.Logging.LogLevel;

namespace Kp.Ms.Oxi.Services;

public class MessageService
{
    private readonly IConfiguration _configuration;
    private readonly DefaultContext _context;
    private readonly LogService _logService;
    private readonly ReservationService _reservationService;
    private readonly QuotaService _quotaService;
    private readonly RateService _rateService;
    private readonly BookingService _bookingService;
    private readonly HttpClient _client;
    private readonly JsonSerializerOptions _jso;
    private readonly BookingApiOptions _bookingOptions;

    public MessageService(
        IConfiguration configuration,
        DefaultContext context,
        LogService logService,
        ReservationService reservationService,
        QuotaService quotaService,
        BookingService bookingService,
        RateService rateService,
        IOptions<BookingApiOptions>  bookingApiOptions)
    {
        _configuration = configuration;
        _context = context;
        _logService = logService;
        _reservationService = reservationService;
        _quotaService = quotaService;
        _rateService = rateService;
        _bookingService = bookingService;
        _client = new HttpClient();
        
        _client.DefaultRequestHeaders.Clear();
        _client.DefaultRequestHeaders.ConnectionClose = true;
        _client.Timeout = TimeSpan.FromSeconds(10);
        
        _jso = new JsonSerializerOptions
        {
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
        };

        _bookingOptions = bookingApiOptions.Value;
    }

    public async Task<bool> Receive(string message, MessageReceiveRequest? messageReceiveRequest)
    {
        JsonSerializerOptions jso = new JsonSerializerOptions
        {
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
        };

        if (messageReceiveRequest != null)
        {
            _logService.Log(LogLevel.Information, $"Query params: \n{JsonSerializer.Serialize(messageReceiveRequest, jso)}");
        }

        XmlDocument xml = new XmlDocument();
        try
        {
            xml.LoadXml(message);
        }
        catch (Exception ex)
        {
            _logService.Log(LogLevel.Information, $"Input message: \n{message}");
            _logService.Log(LogLevel.Error, $"Xml parse error: {ex.Message}");
            return false;
        }

        _logService.LogRawOperaMessage(LogLevel.Information, message);
        _logService.LogOperaMessage(LogLevel.Information, JsonConvert.SerializeXmlNode(xml));

        XmlElement? root = xml.DocumentElement;
        if (root == null)
        {
            _logService.Log(LogLevel.Error, "Root element is missing");
            return false;
        }

        if (root.Name == nameof(Reservation))
        {
            var reservation = DeserializeXmlToObject<Reservation>(xml);

            await UpsertReservationAsu(reservation);

            //await _quotaService.UpdateRoomQuotasByReservation(reservation);
        }

        if (root.Name == nameof(RtavMessage) && root.NamespaceURI.Contains("rtav.fidelio"))
        {
            var rtavMessage = DeserializeXmlToObject<RtavMessage>(xml);
            return await _quotaService.UpdateRoomQuotasByRtavMessage(rtavMessage);
        }

        if (root.Name == nameof(RateHeader) && root.NamespaceURI.Contains("rate.fidelio"))
        {
            return await _rateService.UpdateRatesByRateHeader(xml);
        }

        if (string.IsNullOrEmpty(xml.LastChild?.PreviousSibling?.Value))
        {
            _logService.Log(LogLevel.Error, "Label not exists");
            return false;
        }

        string label = xml.LastChild?.PreviousSibling?.Value!;
        string[] labelParts = label.Split('|');

        using var httpClient = new HttpClient();
        httpClient.DefaultRequestHeaders.Clear();
        httpClient.DefaultRequestHeaders.ConnectionClose = true;
        httpClient.Timeout = TimeSpan.FromSeconds(10);

        string baseUrl;
        string token;
        string transactionGetResponseUrl;
        try
        {
            baseUrl = _configuration["BookingApi:BaseUrl"] ??
                      throw new Exception("Booking api base url not configured");
            token = _configuration["BookingApi:Token"] ??
                    throw new Exception("Booking api token not configured");
            transactionGetResponseUrl = _configuration["BookingApi:TransactionGetResponseUrl"] ??
                                        throw new Exception("Booking api transaction get response not configured");
        }
        catch (Exception ex)
        {
            _logService.Log(LogLevel.Error, $"Configuration error: {ex.Message}");
            return false;
        }

        string transactionId = labelParts[2];
        if (!transactionId.Contains("BK"))
        {
            return false;
        }

        string transactionIdExternal = transactionId.Substring(2);

        string statusFromOpera;
        switch (labelParts[3])
        {
            case "SUCCESS":
            case "ERROR":
                statusFromOpera = labelParts[3].ToLower();
                break;
            case "FAILED":
                statusFromOpera = "fail";
                break;
            default:
                statusFromOpera = "error";
                break;
        }

        TransactionGetResponseRequest request = new TransactionGetResponseRequest
        {
            Token = token,
            TransactionId = transactionIdExternal,
            Status = statusFromOpera,
            Response = message
        };

        // если мы пытаемся апдейтить бронь которой нет в опере, повторно такие попытки делать не надо, поэтому статус error
        // TODO сделать enum на статусы для второго апи
        if (message.Contains("Reservation not found for External system"))
        {
            request.Status = "error";
        }

        XmlElement? xRoot = xml.DocumentElement;
        if (xRoot != null)
        {
            XmlNode? attr = xRoot.Attributes.GetNamedItem("success");
            if (attr?.Value == "SUCCESS")
            {
                foreach (XmlElement xnode in xRoot)
                {
                    if (xnode.Name != "ResultIds") continue;

                    foreach (XmlNode resultId in xnode.ChildNodes)
                    {
                        XmlNode? resultMsgType = resultId.Attributes?.GetNamedItem("resultMsgType");
                        foreach (XmlNode resultIdChild in resultId.ChildNodes)
                        {
                            // skip travel agent
                            if (resultIdChild.Name == "crsId" && resultIdChild.InnerText == "1")
                            {
                                break;
                            }

                            if (resultIdChild.Name == "pmsId")
                            {
                                switch (resultMsgType?.Value)
                                {
                                    case "PROFILE":
                                        request.GuestExternalId = resultIdChild.InnerText;
                                        break;
                                    case "RESERVATION":
                                        request.ReservationExternalId = resultIdChild.InnerText;
                                        break;
                                }
                            }
                        }
                    }
                }
            }
        }

        _logService.Log(LogLevel.Information, $"Request to booking api transaction get response: {JsonSerializer.Serialize(request, jso)}");

        var requestMessage = new HttpRequestMessage(HttpMethod.Post, baseUrl + transactionGetResponseUrl);
        requestMessage.Content = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");
        using var response = await httpClient.SendAsync(requestMessage);
        string apiResponse = await response.Content.ReadAsStringAsync();

        _logService.Log(LogLevel.Information, $"Response of booking api transaction get response: {Regex.Unescape(apiResponse)}");

        List<Transaction> transactions = await _context.Transactions
            .Where(t => t.TransactionId == transactionId && t.Received == false)
            .ToListAsync();
        transactions.ForEach(t => { t.Received = true; t.ReceivedAt = DateTime.UtcNow; });
        await _context.SaveChangesAsync();

        return true;
    }

    private async Task UpsertReservationAsu(Reservation reservation)
    {
        try
        {
            if (reservation == null)
            {
                _logService.Log(LogLevel.Error, "Error processing upsert reservation asu.");
                return;
            }

            var getGuestCount = new Func<ReservationGuestAgeCode, int>(
                (guest) => reservation.GuestCounts?.FirstOrDefault(guestCount => guestCount.AgeQualifyingCode == guest.ToString())?.MfCount ?? 0);

            var hotel = await _bookingService.GetHotel(reservation.HotelReference?.HotelCode);
            var roomType = await _bookingService.GetRoomType(hotel.Id, reservation.RoomStays?.FirstOrDefault()?.roomInventoryCode);

            var existsReservationAsu = await _bookingService.GetReservationAsu(hotel.Id, reservation.ReservationId);

            var reservationAsu = new BookingReservationAsu()
            {
                Id = existsReservationAsu?.Id ?? 0,
                ExternalId = reservation.ReservationId,
                HotelId = hotel?.Id,
                RoomTypeId = roomType?.Id,
                Total = (ulong)reservation.RoomRevenues?.Sum(roomRevenue => roomRevenue?.NetRoomRate?.ValueNum),
                RoomQuantity = reservation.RoomStays?.Count,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = existsReservationAsu != null ? DateTime.UtcNow : null,
                Adults = getGuestCount.Invoke(ReservationGuestAgeCode.ADULT),
                Children = getGuestCount.Invoke(ReservationGuestAgeCode.CHILD),
                Infant = getGuestCount.Invoke(ReservationGuestAgeCode.INFANT),
                Babies = getGuestCount.Invoke(ReservationGuestAgeCode.BABY)
            };

            if (reservation.StayDateRange?.StartTime != null)
            {
                reservationAsu.DateIn = DateOnly.FromDateTime(reservation.StayDateRange.StartTime);
                reservationAsu.DateOut = DateOnly.FromDateTime(reservation.StayDateRange.StartTime.AddDays(reservation.StayDateRange?.NumberOfTimeUnits ?? 0));
            }
            else
            {
                reservationAsu.DateIn = null;
                reservationAsu.DateOut = null;
            }

                await _bookingService.UpsertReservationAsu(reservationAsu);
        }
        catch (Exception ex)
        {
            _logService.Log(LogLevel.Error, $"Failed to upsert reservation asu: {ex.Message}");
        }
    }

    public async Task<OTA_HotelAvailNotifRS?> FidelioMessageXml(string message)
    {
        _logService.Log(LogLevel.Information, $"FidelioMessageXml message: \n{message}");

        XmlDocument xml = new XmlDocument();
        try
        {
            xml.LoadXml(message);
        }
        catch (Exception ex)
        {
            var errorMessage = $"Xml parse error: {ex.Message}";
            _logService.Log(LogLevel.Error, errorMessage);

            return new OTA_HotelAvailNotifRS()
            {
                Errors = new List<Error>() {
                    new Error()
                    {
                        Status = "NotProcessed",
                        Text = errorMessage
                    }
                }
            };
        }

        _logService.LogRawOperaMessage(LogLevel.Information, message);
        _logService.LogOperaMessage(LogLevel.Information, JsonConvert.SerializeXmlNode(xml));

        XmlElement? root = xml.DocumentElement;
        if (root == null)
        {
            var errorMessage = "Root element is missing";
            _logService.Log(LogLevel.Error, errorMessage);

            return new OTA_HotelAvailNotifRS()
            {
                Errors = new List<Error>() {
                    new Error()
                    {
                        Status = "NotProcessed",
                        Text = errorMessage
                    }
                }
            };
        }

        if (root.Name == nameof(OTA_HotelAvailNotifRQ))
        {
            var reservation = DeserializeXmlToObject<OTA_HotelAvailNotifRQ>(xml);

            var errors = await _quotaService.UpdateRoomQuotasByOTAMessage(reservation);

            if (errors != null && errors.Any())
            {
                return new OTA_HotelAvailNotifRS()
                {
                    Errors = errors.Select(error => new Error()
                    {
                        Status = "NotProcessed",
                        Text = error
                    }).ToList()
                };
            }
        }

        return new OTA_HotelAvailNotifRS()
        {
            Success = ""
        };
    }

    public async Task<StatusDto> FidelioResult(string message)
    {
        FidelioResultRequest? request;
        try
        {
            XmlSerializer serializer = new XmlSerializer(typeof(FidelioResultRequest));
            using TextReader reader = new StringReader(message);
            request = serializer.Deserialize(reader) as FidelioResultRequest;
        }
        catch (Exception ex)
        {
            StatusDto dto = new StatusDto
            {
                Status = StatusType.Fail.ToString().ToLower(),
                Message = $"Fidelio result request parse error: {ex.Message}"
            };
            _logService.Log(LogLevel.Error, dto.Message);
            return dto;
        }

        if (request == null)
        {
            StatusDto dto = new StatusDto
            {
                Status = StatusType.Fail.ToString().ToLower(),
                Message = "Fidelio result request is null"
            };
            _logService.Log(LogLevel.Error, dto.Message);
            return dto;
        }
        _logService.Log(LogLevel.Information, $"Fidelio result: \n{JsonSerializer.Serialize(request, _jso)}");

        Transaction? transaction = await _context.Transactions.FirstOrDefaultAsync(t => t.TransactionId == request.Response.Id);
        if (transaction == null)
        {
            StatusDto dto = new StatusDto
            {
                Status = StatusType.Fail.ToString().ToLower(),
                Message = "Fidelio result request transaction ID not found in DB"
            };
            _logService.Log(LogLevel.Error, dto.Message);
            return dto;
        }

        string statusFromFidelio = request.Response.Status == "OK" ? "success" : "error";

        SendReservationRequest? reservationRequest = JsonSerializer.Deserialize<SendReservationRequest>(transaction.Request);
        if (reservationRequest == null)
        {
            StatusDto dto = new StatusDto
            {
                Status = StatusType.Fail.ToString().ToLower(),
                Message = "Dont parse reservation request in transaction"
            };
            _logService.Log(LogLevel.Error, dto.Message);
            return dto;
        }

        var guestExternalId = reservationRequest.Reservation.Profile.ExternalId ??
                              request.Response.InsertResponse?.Rows.ProfileId;

        if (string.IsNullOrWhiteSpace(guestExternalId))
        {
            StatusDto dto = new StatusDto
            {
                Status = StatusType.Fail.ToString().ToLower(),
                Message = "Received empty ExternalId in json and xml Fidelio"
            };
            _logService.Log(LogLevel.Error, dto.Message);
            return dto;
        }

        TransactionGetResponseRequest requestToBk = new TransactionGetResponseRequest
        {
            Token = _bookingOptions.Token,
            TransactionId = reservationRequest.Interface.TransactionId.Substring(2),
            Status = statusFromFidelio,
            Response = message,
            GuestExternalId = guestExternalId
        };

        if (statusFromFidelio == "success")
        {
            if (request.Response.InsertResponse != null)
            {
                if (request.Response.InsertResponse.Rows.GuestNum == null)
                {
                    StatusDto dto = new StatusDto
                    {
                        Status = StatusType.Fail.ToString().ToLower(),
                        Message = "Response from fidelio dont contain GuestNum"
                    };
                    _logService.Log(LogLevel.Error, dto.Message);
                    return dto;
                }

                requestToBk.ReservationExternalId = request.Response.InsertResponse.Rows.GuestNum.ToString();
            }
            else if (request.Response.UpdateResponse != null || reservationRequest.Reservation.ExternalId != null)
            {
                requestToBk.ReservationExternalId = reservationRequest.Reservation.ExternalId;
            }
        }

        _logService.Log(LogLevel.Information, $"Request to booking api transaction get response url {_bookingOptions.BaseUrl + _bookingOptions.TransactionGetResponseUrl} body: {JsonSerializer.Serialize(requestToBk, _jso)}");

        var requestMessage = new HttpRequestMessage(HttpMethod.Post, _bookingOptions.BaseUrl + _bookingOptions.TransactionGetResponseUrl);
        requestMessage.Content = new StringContent(JsonSerializer.Serialize(requestToBk, _jso), Encoding.UTF8, "application/json");
        using var response = await _client.SendAsync(requestMessage);
        string apiResponse = await response.Content.ReadAsStringAsync();

        _logService.Log(LogLevel.Information, $"Response from booking api transaction get response: {Regex.Unescape(apiResponse)}");

        transaction.Received = true;
        transaction.ReceivedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();

        return new StatusDto { Status = StatusType.Success.ToString().ToLower() };
    }

    private TModel DeserializeXmlToObject<TModel>(XmlDocument xmlDocument) where TModel : class
    {
        try
        {
            var rootNode = xmlDocument.DocumentElement;
            if (rootNode == null)
            {
                return null;
            }

            XmlSerializer serializer = new XmlSerializer(typeof(TModel));
            using var reader = new StringReader(rootNode.OuterXml);
            using var xmlReader = XmlReader.Create(reader);

            return (TModel?)serializer.Deserialize(xmlReader);
        }
        catch (Exception ex)
        {
            _logService.Log(LogLevel.Error, $"XML deserialization error: {ex.Message}");
            return null;
        }
    }
}