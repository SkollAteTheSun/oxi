using System.Text;
using System.Xml;
using System.Xml.Serialization;
using Kp.Ms.Oxi.Context;
using Kp.Ms.Oxi.Models.Opera;
using Kp.Ms.Oxi.Models.Request;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Kp.Ms.Oxi.Services;

public class RateService
{
    private readonly IConfiguration _configuration;
    private readonly ILogger _logger;
    private readonly IDbContextFactory<BookingContext> _contextFactory;

    public RateService(IConfiguration configuration, ILogger<RateService> logger, IDbContextFactory<BookingContext> contextFactory)
    {
        _configuration = configuration;
        _logger = logger;
        _contextFactory = contextFactory;
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
        var vectors = context.RoomRatesVectorAggRows.FromSqlRaw(query).ToList();

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
}