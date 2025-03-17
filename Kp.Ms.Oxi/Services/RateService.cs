using System.Text;
using System.Text.Json;
using System.Xml;
using System.Xml.Serialization;
using Kp.Ms.Oxi.Context;
using Kp.Ms.Oxi.Models.Entity;
using Kp.Ms.Oxi.Models.Opera;
using Kp.Ms.Oxi.Models.Request;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json.Linq;

namespace Kp.Ms.Oxi.Services;

public class RateService
{
    private readonly IConfiguration _configuration;
    private readonly LogService _logService;
    private readonly IDbContextFactory<BookingContext> _contextFactory;
    private readonly XmlDeserializer _xmlDeserializer;
    private readonly BookingService _bookingService;
    private readonly int _chunkSize;

    public RateService(IConfiguration configuration, LogService logService, IDbContextFactory<BookingContext> contextFactory, 
                        XmlDeserializer xmlDeserializer, BookingService bookingService)
    {
        _configuration = configuration;
        _logService = logService;
        _contextFactory = contextFactory;
        _xmlDeserializer = xmlDeserializer;
        _bookingService = bookingService;
        _chunkSize = configuration.GetValue<int?>("Processing:ChunkSize") ?? throw new ArgumentNullException("Processing");
    }

    public async Task<string> MakeRateSet(RateSetRequest request)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();

        string dateStart = request.DateStart.ToString("yyyy-MM-dd");
        string dateEnd = request.DateEnd.ToString("yyyy-MM-dd");

        string roomTypesIdsImpl = string.Join(",", request.RoomTypeIds);

        string query = $"""
                        SELECT
                            r.code as rate_code
                            , v.rate_id
                            , rt.code as room_type_code
                            , v.room_type_id
                            , v.date_start
                            , v.date_end
                        	, max(if(v.adults = 1, v.base_price, 0)) as rate1
                        	, max(if(v.adults = 2, if(v.extra_bed, v.base_price + v.adult_price_on_beds + v.children_price_on_beds, v.base_price), 0)) as rate2
                        	, max(if(v.adults = 3, if(v.extra_bed, v.base_price + v.adult_price_on_beds + v.children_price_on_beds, v.base_price), 0)) as rate3
                        	, max(if(v.adults = 4, if(v.extra_bed, v.base_price + v.adult_price_on_beds + v.children_price_on_beds, v.base_price), 0)) as rate4
                        	, max(if(v.adults = 5, if(v.extra_bed, v.base_price + v.adult_price_on_beds + v.children_price_on_beds, v.base_price), 0)) as rate5
                        FROM kpresort_bk.room_rates_vector_{request.HotelId} v
                        JOIN kpresort_bk.rates r on r.id = v.rate_id
                        JOIN kpresort_bk.room_types rt on rt.id = v.room_type_id
                        WHERE
                        	v.rate_id = {request.RateId}
                        	and v.date_start >= '{dateStart}'
                        	and v.date_end < '{dateEnd}'
                        	and v.room_type_id in ({roomTypesIdsImpl})
                        GROUP BY v.date_start, v.date_end, v.room_type_id
                        ORDER BY v.date_start, v.date_end
                        """;

//         string query = $"""
//                                                         select
//                                                         r.code as rate_code,
//                                                         v.rate_id,
//                                                         rt.code as room_type_code,
//                                                         v.room_type_id,
//                                                         v.date_start,
//                                                         v.date_end,
//                                                         v.adults,
//                                                         v.children,
//                                                         v.babies,
//                                                         v.amount,
//                                                         v.extra_bed,
//                                                         v.craddle,
//                                                         v.base_price,
//                                                         v.baby_price,
//                                                         v.craddle_price,
//                                                         v.children_price,
//                                                         v.baby_price_on_beds,
//                                                         v.adult_price_on_beds,
//                                                         v.children_price_on_beds
//                                                             from room_rates_vector_{request.HotelId} v
//                                                             join rates r on r.id = v.rate_id
//                                                         join room_types rt on rt.id = v.room_type_id
//                                                         where 1 = 1
//                                                         and v.rate_id = {request.RateId}
//                                                         and v.date_start >= '{dateStart}'
//                                                         and v.date_end <= '{dateEnd}'
//                                                         and v.room_type_id IN ({roomTypesIdsImpl})
//                         """;

        // var vectors = context.RoomRatesVectorRows.FromSqlRaw(query).ToList();
        var vectors = context.Database.SqlQueryRaw<RoomRatesVectorAgg>(query).ToList();

        string? rateCode = null;
        List<RateDetail> rateDetails = [];
        foreach (var oneVector in vectors)
        {
            rateCode ??= oneVector.RateCode;

            rateDetails.Add(new RateDetail
            {
                RateCode = oneVector.RateCode,
                DateStart = oneVector.DateStart.ToString("yyyy-MM-dd"),
                DateEnd = oneVector.DateEnd.ToString("yyyy-MM-dd"),
                RoomTypes = [oneVector.RoomTypeCode],
                MarketCode = "NEW",
                Rate1 = oneVector.Rate1,
                Rate2 = oneVector.Rate2,
                Rate3 = oneVector.Rate3,
                Rate4 = oneVector.Rate4,
                Rate5 = oneVector.Rate5,
                Child1RateParentRoom = 0,
                Child2RateParentRoom = 0,
                Child3RateParentRoom = 0,
                Child1RateOwnRoom = 0,
                Child2RateOwnRoom = 0,
                Child3RateOwnRoom = 0,
                Child4RateOwnRoom = 0,
                ExtraBed = 0,
                DaysOfWeek = new RateDetailDaysOfWeek
                {
                    Monday = 1,
                    Tuesday = 1,
                    Wednesday = 1,
                    Thursday = 1,
                    Friday = 1,
                    Saturday = 1,
                    Sunday = 1
                },
                PayingChildren = 3,
                FreeChildren = 1,
            });
        }
        // rateDetails.Add(new RateDetail
        // {
        //     RateCode = "ALEXEY",
        //     DateStart = dateStart,
        //     DateEnd = dateEnd,
        //     RoomTypes = ["KGB", "KGBMV"],
        //     MarketCode = "NEW",
        //     Rate1 = 0,
        //     Rate2 = 0,
        //     Rate3 = 0,
        //     Rate4 = 0,
        //     Rate5 = 0,
        //     Child1RateParentRoom = 0,
        //     Child2RateParentRoom = 0,
        //     Child3RateParentRoom = 0,
        //     Child1RateOwnRoom = 0,
        //     Child2RateOwnRoom = 0,
        //     Child3RateOwnRoom = 0,
        //     Child4RateOwnRoom = 0,
        //     ExtraBed = 0,
        //     DaysOfWeek = new RateDetailDaysOfWeek
        //     {
        //         Monday = 1,
        //         Tuesday = 1,
        //         Wednesday = 1,
        //         Thursday = 1,
        //         Friday = 1,
        //         Saturday = 1,
        //         Sunday = 1
        //     },
        //     PayingChildren = 3,
        //     FreeChildren = 1,
        // });

        var rateSet = new RateSet
        {
            RateStatus = "ACTIVE",
            YieldStatus = "NONE",
            RateMessageType = "DETAIL",
            HotelCode = "B755",
            RateCode = rateCode ?? string.Empty,
            RateDetails = rateDetails,
            DbaseFltPct = "FLT",
            DbaseRounding = "N",
            DbaseCompare = 0,
            AdvBaseRounding = "N"
        };

        XmlSerializerNamespaces ns = new XmlSerializerNamespaces([XmlQualifiedName.Empty]);

        XmlSerializer xmlSerializer = new XmlSerializer(typeof(RateSet));
        string xmlBody = "";

        XmlWriterSettings settings = new XmlWriterSettings
        {
            OmitXmlDeclaration = true
        };

        string? apiResponse = null;

        using (var sww = new StringWriter())
        {
            using (XmlWriter writer = XmlWriter.Create(sww, settings))
            {
                xmlSerializer.Serialize(writer, rateSet, ns);
                xmlBody = sww.ToString();

                string xmlHeader = $"""
                                    <?xml version = '1.0'?>
                                    <?Label {rateSet.HotelCode}|RATE|{request.TransactionId}|NEW?>
                                    """;

                StringBuilder sb = new StringBuilder();
                sb.Append(xmlHeader);
                sb.Append('\n');
                sb.Append(xmlBody);

                var xml = sb.ToString();

                using (var httpClient = new HttpClient())
                {
                    // httpClient.BaseAddress = new Uri("http://10.100.212.40/Operajserv/OXIServlets/PMSInterface");
                    httpClient.DefaultRequestHeaders.Clear();
                    httpClient.DefaultRequestHeaders.ConnectionClose = true;
                    httpClient.Timeout = TimeSpan.FromSeconds(10);

                    var queryParams = new Dictionary<string, string>
                    {
                        { "interfaceName", "TARS" },
                        { "propertyName", "B755" },
                        { "messageType", "RATE" },
                        { "transactionId", request.TransactionId.ToString() },
                        { "status", "NEW" },
                    };
                    // var requestUri = QueryHelpers.AddQueryString("", queryParams);

                    var queryBuilder = new QueryBuilder(queryParams);
                    string basePath = "http://10.100.212.40/Operajserv/OXIServlets/PMSInterface";

                    var requestMessage = new HttpRequestMessage(HttpMethod.Post, basePath + queryBuilder);
                    requestMessage.Content = new StringContent(xml, Encoding.UTF8, "text/xml");
                    using (var response = httpClient.SendAsync(requestMessage).Result)
                    {
                        response.EnsureSuccessStatusCode();
                        apiResponse = response.Content.ReadAsStringAsync().Result;
                    }
                }
            }
        }

        return apiResponse;
    }

    /*
    public async Task<bool> UpdateRatesByRateHeader(XmlDocument xml)
    {
        var rateHeader = _xmlDeserializer.DeserializeXmlToMessage<RateHeader>(xml);
        if (rateHeader == null)
        {
            _logService.Log(LogLevel.Error, "Error processing RateHeader message.");
            return false;
        }

        var hotelId = await _bookingService.GetHotelId(rateHeader.HotelCode);
        if (!hotelId.HasValue)
        {
            _logService.Log(LogLevel.Warning, $"Hotel: {rateHeader.HotelCode} not found in database");
            return false;
        }

        var roomTypeDict = await _bookingService.GetRoomTypesByHrsCodes(hotelId.Value, rateHeader.RoomTypeList);

        var rateId = await _bookingService.GetRateId(rateHeader.RateCode, hotelId.Value);
        if (!rateId.HasValue)
        {
            _logService.Log(LogLevel.Warning, $"Rate: {rateHeader.RateCode} not found in database for hotelID: {hotelId.Value}.");
            return false;
        }

        var ratesToUpdate = new List<BookingRoomRates>();
        var semaphore = new SemaphoreSlim(10);
        var tasks = new List<Task>();

        //получить все параметры в List или Dict из рум тупес    

        foreach (var rateDetail in rateHeader.RateDetails.RateDetail)
        {
            if (!DateOnly.TryParse(rateDetail.DateStart, out var startDate))
            {
                _logService.Log(LogLevel.Error, $"Invalid start date: {rateDetail.DateStart}");
                continue;
            }

            if (!DateOnly.TryParse(rateDetail.DateEnd, out var endDate))
            {
                _logService.Log(LogLevel.Error, $"Invalid end date: {rateDetail.DateEnd}");
                continue;
            }

            var dates = new List<DateOnly>();
            for (var date = startDate; date <= endDate; date = date.AddDays(1))
            {
                dates.Add(date);
            }

            foreach (var date in dates)
            {
                foreach (var roomTypeCode in rateDetail.RoomTypes)
                {
                    //по каждому коду roomTypeCode, который нашли в базе
                    if (!roomTypeDict.TryGetValue(roomTypeCode, out var roomType))
                    {
                        _logService.Log(LogLevel.Warning, $"Room type {roomTypeCode} not found in database for hotelID: {hotelId.Value}.");
                        continue;
                    }

                    var rates = GenerateRates(rateDetail);

                    foreach (var rate in rates)
                    {
                        ratesToUpdate.Add(new BookingRoomRates
                        {
                            HotelId = (long)hotelId.Value,
                            RoomTypeId = (long)roomType,
                            RateId = (long)rateId.Value,
                            Date = date,
                            Adults = rate.Adults,
                            Children = rate.Children,
                            Babies = rate.Babies,
                            ExtraBed = rate.ExtraBed,
                            Craddle = rate.Cradle,
                            Active = true,
                            Amount = rate.Amount,
                            BasePrice = rate.BasePrice,

                        });
                    }
                }
            }
        }
        await ProcessChunksAsync(ratesToUpdate, _chunkSize, (long)hotelId);
        return true;
    }
    */

    public async Task<bool> UpdateRatesByRateHeader(XmlDocument xml)
    {
        var rateHeader = _xmlDeserializer.DeserializeXmlToMessage<RateHeader>(xml);
        if (rateHeader == null)
        {
            _logService.Log(LogLevel.Error, "Error processing RateHeader message.");
            return false;
        }

        var hotelId = await _bookingService.GetHotelId(rateHeader.HotelCode);
        if (!hotelId.HasValue)
        {
            _logService.Log(LogLevel.Warning, $"Hotel: {rateHeader.HotelCode} not found in database");
            return false;
        }

        var roomTypeDict = await _bookingService.GetRoomTypesByHrsCodes2(hotelId.Value, rateHeader.RoomTypeList);

        var rateId = await _bookingService.GetRateId(rateHeader.RateCode, hotelId.Value);
        if (!rateId.HasValue)
        {
            _logService.Log(LogLevel.Warning, $"Rate: {rateHeader.RateCode} not found in database for hotelID: {hotelId.Value}.");
            return false;
        }

        var ratesToUpdate = new List<BookingRoomRates>();
   
        foreach (var rateDetail in rateHeader.RateDetails.RateDetail)
        {
            if (!DateOnly.TryParse(rateDetail.DateStart, out var startDate))
            {
                _logService.Log(LogLevel.Error, $"Invalid start date: {rateDetail.DateStart}");
                continue;
            }

            if (!DateOnly.TryParse(rateDetail.DateEnd, out var endDate))
            {
                _logService.Log(LogLevel.Error, $"Invalid end date: {rateDetail.DateEnd}");
                continue;
            }

            var dates = new List<DateOnly>();
            for (var date = startDate; date <= endDate; date = date.AddDays(1))
            {
                dates.Add(date);
            }

            foreach (var date in dates)
            {
                foreach (var roomTypeCode in rateDetail.RoomTypes)
                {
                    //по каждому коду roomType, который нашли в базе
                    if (!roomTypeDict.TryGetValue(roomTypeCode, out var roomType))
                    {
                        _logService.Log(LogLevel.Warning, $"Room type {roomTypeCode} not found in database for hotelID: {hotelId.Value}.");
                        continue;
                    }

                    var rates = GenerateRatesNEW(rateDetail, roomType);

                    foreach (var rate in rates)
                    {
                        ratesToUpdate.Add(new BookingRoomRates
                        {
                            HotelId = (long)hotelId.Value,
                            RoomTypeId = (long)roomType.Id,
                            RateId = (long)rateId.Value,
                            Date = date,
                            Adults = rate.Adults,
                            Children = rate.Children,
                            Babies = rate.Babies,
                            ExtraBed = rate.ExtraBed,
                            Craddle = rate.Cradle,
                            Active = true,
                            Amount = rate.Amount,
                            BasePrice = rate.BasePrice,

                        });
                    }
                }
            }
        }
        await ProcessChunksAsync(ratesToUpdate, _chunkSize, (long)hotelId);
        return true;
    }

    private List<RateInfo> GenerateRates(RateDetail rateDetail)
    {
        var rates = new List<RateInfo>();

        ulong[] adultRates = { rateDetail.Rate1, rateDetail.Rate2 ?? 0, rateDetail.Rate3 ?? 0, rateDetail.Rate4 ?? 0, rateDetail.Rate5 ?? 0 };
        ulong[] childRates = { rateDetail.Child1RateOwnRoom ?? 0, rateDetail.Child2RateOwnRoom ?? 0, rateDetail.Child3RateOwnRoom ?? 0, rateDetail.Child4RateOwnRoom ?? 0 };

        for (byte adults = 1; adults <=5; adults++) // по каждому тарифу для взрослого
        {
            ulong adultRate = adultRates[adults - 1];

            int basePrice = 0;
            if (adultRates[adults - 1] > Int32.MaxValue)
            {
                _logService.Log(LogLevel.Error, $"Error processing rate {rateDetail.RateCode}. Rate1 value ({rateDetail.Rate1}) > int max value");
            }
            else
            {
                basePrice = Convert.ToInt32(adultRate);
            }

            if (adultRate > 0)
            {
                rates.Add(new RateInfo(adults, 0, 0, basePrice, (long)adultRate, 0, 0)); 
                 
                for (byte children = 1; children <= 4; children++) //добавляем детей
                {
                    ulong childRate = childRates[children - 1];

                    if (childRate > 0)
                    {
                        rates.Add(new RateInfo(adults, children, 0, basePrice, (long)(adultRate + childRate), 0, 0));
                    }
                }
            }
        }

        if (rateDetail.ExtraBed > 0) // цену за доп место добавляем ко всем тарифам
        {
            foreach (var rate in rates.ToList())
            {
                rates.Add(new RateInfo(rate.Adults, rate.Children, rate.Babies, rate.BasePrice, rate.Amount + (long)rateDetail.ExtraBed, 1, 0));
            }
        }

        // пока не нашли babies и cradle в Opera PMS
        foreach (var rate in rates.ToList())
        {
            rates.Add(new RateInfo(rate.Adults, rate.Children, 1, rate.BasePrice, rate.Amount, rate.ExtraBed, 0));
            rates.Add(new RateInfo(rate.Adults, rate.Children, 1, rate.BasePrice, rate.Amount, rate.ExtraBed, 1));
        }

        return rates;
    }

    //---------------------------------------------------------------------------------------------------

    /*
    private List<RateInfo> GenerateRates(RateDetail rateDetail, BookingRoomTypes roomType)
    {
        var rates = new List<RateInfo>();

        // Общая вместимость номера
        byte totalCapacity = (byte)(roomType.CountMainSeat + (roomType.ExtraBedCapacity ?? 0));

        int babiesCapacity = 0;
        int childrenCapacity = 0;
        if (!string.IsNullOrEmpty(roomType.Data))
        {
            try
            {
                var dataJson = JsonSerializer.Deserialize<Dictionary<string, string>>(roomType.Data);
                if (dataJson != null && dataJson.TryGetValue("babies_capacity", out var babiesValue))
                {
                    babiesCapacity = int.TryParse(babiesValue, out int parsedValue) ? parsedValue : 0;
                }
            }
            catch (JsonException ex)
            {
                _logService.Log(LogLevel.Error, $"Error parsing Data field for room type {roomType.Code}: {ex.Message}");
            }
        }

        // Если true, то детей не учитываем
        if (roomType.NotAvailForBaby)
        {
            babiesCapacity = 0;
        }

        // Если  true, то подростков не учитываем
        if (roomType.NotAvailForChildren)
        {
            childrenCapacity = 0;
        }

        // Устанавливаем тарифы для взрослых
        ulong[] adultRates = { rateDetail.Rate1, rateDetail.Rate2 ?? 0, rateDetail.Rate3 ?? 0, rateDetail.Rate4 ?? 0, rateDetail.Rate5 ?? 0 };
        //Устанавливаем тарифы для подростков
        ulong[] childRates = { rateDetail.Child1RateOwnRoom ?? 0, rateDetail.Child2RateOwnRoom ?? 0, rateDetail.Child3RateOwnRoom ?? 0, rateDetail.Child4RateOwnRoom ?? 0 };

        for (byte adults = 1; adults <= totalCapacity; adults++)
        {
            ulong adultRate = adultRates[adults - 1];

            int basePrice = 0;
            if (adultRates[adults - 1] > Int32.MaxValue)
            {
                _logService.Log(LogLevel.Error, $"Error processing rate {rateDetail.RateCode}. Rate1 value ({rateDetail.Rate1}) > int max value");
            }
            else
            {
                basePrice = Convert.ToInt32(adultRate);
            }

            if (adultRate > 0)
            {
                rates.Add(new RateInfo(adults, 0, 0, basePrice, (long)adultRate, 0, 0));

                // Добавляем подростков, если они разрешены
                for (byte children = 1; children <= childrenCapacity; children++)
                {
                    ulong childRate = childRates[children - 1];

                    if (childRate > 0)
                    {
                        rates.Add(new RateInfo(adults, children, 0, basePrice, (long)(adultRate + childRate), 0, 0));
                    }
                }
            }
        }

        // Добавляем младенцев, если разрешены
        // Тут кстати babies не нашли в Opera PMS, поэтому с ценой ничего не далаем
        if (babiesCapacity > 0)
        {
            foreach (var rate in rates.ToList())
            {
                for (byte babies = 1; babies <= babiesCapacity; babies++)
                {
                    byte cradle = babies;
                    rates.Add(new RateInfo(rate.Adults, rate.Children, babies, rate.BasePrice, rate.Amount, rate.ExtraBed, cradle));
                }
            }
        }

        return rates;
    }
    */

    private List<RateInfo> GenerateRatesNEW(RateDetail rateDetail, BookingRoomTypes roomType)
    {
        var rates = new List<RateInfo>();

        bool allowChildren = !roomType.NotAvailForChildren;
        bool allowBabies = !roomType.NotAvailForBaby;

        int mainSeats = roomType.CountMainSeat;
        int extraBedCapacity = roomType.ExtraBedCapacity ?? 0;

        ulong[] adultRates = { rateDetail.Rate1, rateDetail.Rate2 ?? 0, rateDetail.Rate3 ?? 0, rateDetail.Rate4 ?? 0, rateDetail.Rate5 ?? 0 };
        ulong[] childRates = { rateDetail.Child1RateOwnRoom ?? 0, rateDetail.Child2RateOwnRoom ?? 0, rateDetail.Child3RateOwnRoom ?? 0, rateDetail.Child4RateOwnRoom ?? 0 };

        //Основные места
        for (byte adults = 1; adults <= mainSeats; adults++)
        {
            // Получаем тариф для взрослых
            ulong adultRate = adultRates[adults - 1];

            if (adultRate > int.MaxValue)
            {
                _logService.Log(LogLevel.Error, $"Error processing rate {rateDetail.RateCode}. Rate1 value ({rateDetail.Rate1}) > int max value");
                continue;
            }

            int basePrice = (int)adultRate;

            // Перебираем количество детей
            for (byte children = 0; allowChildren && children <= mainSeats - adults; children++)
            {
                ulong childRate = 0;
                if (children > 0)
                {
                    childRate = childRates[children - 1];
                }

                // Если есть дети, но тариф для них 0 — пропускаем итерацию
                if (children > 0 && childRate == 0) continue;

                // Перебираем количество младенцев
                for (byte babies = 0; allowBabies && babies <= mainSeats - adults - children; babies++)
                {
                    rates.Add(new RateInfo(adults, children, babies, basePrice, (long)(adultRate + childRate), 0, 0));
                }
            }
        }

        // Доп места
        if (extraBedCapacity > 0 && rateDetail.ExtraBed > 0)
        {
            var ratesWithExtra = new List<RateInfo>();

            foreach (var rate in rates)
            {
                if (rate.Adults + rate.Children + rate.Babies < mainSeats + extraBedCapacity)
                {
                    ratesWithExtra.Add(new RateInfo(rate.Adults, rate.Children, rate.Babies, rate.BasePrice, rate.Amount + (long)rateDetail.ExtraBed, 1, 0));
                }
            }

            rates.AddRange(ratesWithExtra);
        }

        return rates;
    }

    private async Task ProcessChunksAsync(List<BookingRoomRates> rates, int chunkSize, long hotelId)
    {
        var semaphore = new SemaphoreSlim(10);
        var tasks = new List<Task>();
        var tableName = $"room_rates_{hotelId}_asu";

        foreach (var chunk in rates.Chunk(chunkSize))
        {
            await semaphore.WaitAsync();
            tasks.Add(Task.Run(async () =>
            {
                try
                {
                    await _bookingService.UpdateRateDetails(chunk.ToList(), tableName);
                }
                catch (Exception ex)
                {
                    _logService.Log(LogLevel.Error, $"Failed to update chunk: {ex.Message}");
                }
                finally
                {
                    semaphore.Release();
                }
            }));
        }

        await Task.WhenAll(tasks);
    }
}