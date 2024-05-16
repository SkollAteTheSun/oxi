using System.Text;
using System.Xml;
using System.Xml.Serialization;
using Kp.Ms.Oxi.Models.Opera;
using Kp.Ms.Oxi.Models.Request;
using Microsoft.AspNetCore.Http.Extensions;

namespace Kp.Ms.Oxi.Services;

public class ReservationService
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<ReservationService> _logger;

    public ReservationService(IConfiguration configuration, ILogger<ReservationService> logger)
    {
        _configuration = configuration;
        _logger = logger;
    }

    public async Task<string> Send(CreateReservationRequest request)
    {
        List<Rate> rates = [];
        int rateNum = 0;
        rates.AddRange(request.Reservation.RoomStay.Rates.Select(rate => new Rate
        {
            reservationActionType = "NEW",
            rateBasisTimeUnitType = "DAY",
            rateRPH = rateNum++,
            Amount = new Amount { CurrencyCode = "RUB", ValueNum = rate.Amount },
            rateBasisUnits = 1,
            TimeSpan = new TimeUnit { TimeUnitType = "DAY", StartTime = rate.Date, NumberOfTimeUnits = 1 },
            mfAdults = request.Reservation.Adult,
            mfChildren = request.Reservation.Child,
            mfsourceCode = request.Reservation.RoomStay.SourceCode,
            mfMarketCode = request.Reservation.RoomStay.MarketCode,
            Discount = new Discount { Type = "AMOUNT", ValueNum = rate.Discount },
            rateRoomType = request.Reservation.RoomTypeCode,
            mfChildren1 = 0,
            mfChildren2 = 0,
            mfChildren3 = 0,
            mfChildren4 = 0,
            mfChildren5 = 0
        }));

        Reservation reservation = new Reservation
        {
            MfShareAction = "NA",
            MfReservationAction = "ADD",
            HotelReference = new HotelReference
            {
                HotelCode = request.Interfase.HotelCode
            },
            ReservationId = request.Reservation.ExternalId ?? string.Empty,
            ReservationOriginatorCode = "BOOKING",
            MfLegNumbers = 1,
            OriginalBookingDate = request.Reservation.CreatedAt,
            StayDateRange = new TimeUnit
            {
                TimeUnitType = "DAY",
                StartTime = request.Reservation.DateIn,
                NumberOfTimeUnits = request.Reservation.DateOut.Subtract(request.Reservation.DateIn).Days
            },
            GuestCounts =
            [
                new GuestCount
                {
                    AgeQualifyingCode = "ADULT",
                    MfCount = request.Reservation.Adult
                },

                new GuestCount
                {
                    AgeQualifyingCode = "CHILD",
                    MfCount = request.Reservation.Child
                }
            ],
            ResComments =
            [
                new ResComment
                {
                    ReservationActionType = "NEW",
                    ResCommentRPH = 0,
                    СommentOriginatorCode = "SYSTEM",
                    GuestViewable = 1,
                    Comment = "RATE_FIXED=NO",
                    MfcommentType = "SYSTEM",
                    MfcommentDate = request.Reservation.CreatedAt
                },
                new ResComment
                {
                    ReservationActionType = "NEW",
                    ResCommentRPH = 1,
                    СommentOriginatorCode = "SYSTEM",
                    GuestViewable = 1,
                    Comment = "DISCOUNT_PERCENTAGE=0",
                    MfcommentType = "SYSTEM",
                    MfcommentDate = request.Reservation.CreatedAt
                },
                new ResComment
                {
                    ReservationActionType = "NEW",
                    ResCommentRPH = 2,
                    СommentOriginatorCode = "SYSTEM",
                    GuestViewable = 1,
                    Comment = "PARTY=",
                    MfcommentType = "SYSTEM",
                    MfcommentDate = request.Reservation.CreatedAt
                },
                new ResComment
                {
                    ReservationActionType = "NEW",
                    ResCommentRPH = 3,
                    СommentOriginatorCode = "SYSTEM",
                    GuestViewable = 1,
                    Comment = $"HOTEL_DATE={request.Reservation.DateIn}",
                    MfcommentType = "SYSTEM",
                    MfcommentDate = request.Reservation.CreatedAt
                }
            ],
            ResGuests =
            [
                new ResGuest
                {
                    ReservationActionType = "NEW",
                    ResGuestRph = 0,
                    ProfileRphs = "0, 1",
                    InHouseTimeSpan = new TimeUnit
                    {
                        TimeUnitType = "DAY",
                        StartTime = request.Reservation.DateIn,
                        NumberOfTimeUnits = request.Reservation.DateOut.Subtract(request.Reservation.DateIn).Days
                    },
                    ArrivalTransport = new Transport
                    {
                        TransportTime = request.Reservation.DateIn
                    },
                    DepartureTransport = new Transport
                    {
                        TransportTime = request.Reservation.DateOut
                    },
                    ArrivalTime = request.Reservation.DateIn,
                    DepartureTime = request.Reservation.DateOut,
                    ReservationId = request.Reservation.ExternalId ?? string.Empty,
                    MfConfirmationLegNo = 1,
                    ReservationReferences =
                    [
                        new ReservationReference
                        {
                            Type = "GUESTID",
                            ReferenceNumber = request.Reservation.Profile.Id.ToString(),
                            LegNumber = 1
                        },
                        new ReservationReference
                        {
                            Type = "PMSID",
                            ReferenceNumber = request.Reservation.ReservationNumber,
                            LegNumber = 1
                        }
                    ],
                    PreRegistered = 0,
                    CommissionPaidTo = "N"
                }
            ],
            ResProfiles =
            [
                new ResProfile
                {
                    Profile = new Profile
                    {
                        ProfileType = "CONTACT",
                        Gender = "UNKNOWN",
                        MultiProperty = 0,
                        MiniProfile = 0,
                        PseudoProfile = 0,
                        ReplaceAddress = 0,
                        ProfileId = request.Reservation.Profile.Id,
                        CreatorCode = "SUPERVISOR",
                        CreatedDate = request.Reservation.Profile.CreatedAt,
                        LastUpdaterCode = "ORS",
                        LastUpdated = request.Reservation.Profile.UpdatedAt,
                        GenericName = request.Reservation.Profile.LastName,
                        IndividualName = new IndividualName
                        {
                            NameFirst = request.Reservation.Profile.FirstName,
                            NameSur = request.Reservation.Profile.SurName,
                        },
                        DateOfBirth = request.Reservation.Profile.DateOfBirth,
                        PrimaryLanguageId = "R",
                        MfResort = request.Interfase.HotelCode,
                        MfResortProfileId = request.Reservation.Profile.ExternalId ?? string.Empty,
                        MfNameCode = "112233",
                        MfAllowMail = "YES",
                        MfAllowEMail = "NO",
                        MfGuestPriv = "NO",
                        ElectronicAddresses =
                        [
                            new ElectronicAddress
                            {
                                ElectronicAddressType = "EMAILP",
                                Address = request.Reservation.Profile.Email,
                                MfPrimary = "Y",
                                Confirmation = 0
                            }
                        ],
                        PhoneNumbers =
                        [
                            new PhoneNumber
                            {
                                PhoneNumberType = "MOBILE",
                                Number = request.Reservation.Profile.Phone,
                                MfPrimary = "Y",
                                Confirmation = 0
                            }
                        ],
                        MfAllowPhone = 0,
                        MfAllowSMS = 0,
                        PrivacyOption = new PrivacyOption
                        {
                            mfAllowMail = "Y",
                            mfAllowEMail = "Y",
                            mfAllowPhone = 1,
                            mfAllowSMS = 1,
                            mfAllowHistory = 1,
                            mfAllowMarketResearch = 1,
                            mfAllowThirdParty = 1
                        }
                    },
                    resProfileRPH = 0
                },
                new ResProfile
                {
                    Profile = new Profile
                    {
                        ProfileType = "TRAVEL",
                        Gender = "UNKNOWN",
                        MultiProperty = 0,
                        MiniProfile = 0,
                        PseudoProfile = 0,
                        ReplaceAddress = 0,
                        ProfileId = 1,
                        CreatorCode = "SUPERVISOR",
                        CreatedDate = new DateTime(2020, 1, 1),
                        LastUpdaterCode = "ORS",
                        LastUpdated = new DateTime(2020, 1, 1),
                        GenericName = "KP Reservation",
                        IndividualName = new IndividualName
                        {
                            NameFirst = "KP Reservation",
                            NameSur = "KP Reservation",
                        },
                        MfNameCode = "KP Reservation",
                        MfAllowMail = "NO",
                        MfAllowEMail = "NO",
                        MfGuestPriv = "NO",
                        MfAllowPhone = 0,
                        MfAllowSMS = 0,
                        PrivacyOption = new PrivacyOption
                        {
                            mfAllowMail = "N",
                            mfAllowEMail = "N",
                            mfAllowPhone = 0,
                            mfAllowSMS = 0,
                            mfAllowHistory = 0,
                            mfAllowMarketResearch = 0,
                            mfAllowThirdParty = 0
                        }
                    },
                    resProfileRPH = 1
                }
            ],
            RoomStays =
            [
                new RoomStay
                {
                    mfShareAction = "NA",
                    mfReservationAction = "ADD",
                    reservationActionType = "NEW",
                    reservationStatusType = "RESERVED",
                    roomStayRPH = 0,
                    roomInventoryCode = request.Reservation.RoomTypeCode,
                    TimeSpan = new TimeUnit
                    {
                        TimeUnitType = "DAY",
                        StartTime = request.Reservation.DateIn,
                        NumberOfTimeUnits = request.Reservation.DateOut.Subtract(request.Reservation.DateIn).Days
                    },
                    GuestCounts =
                    [
                        new GuestCount
                        {
                            AgeQualifyingCode = "ADULT",
                            MfCount = request.Reservation.Adult
                        },
                        new GuestCount
                        {
                            AgeQualifyingCode = "CHILD",
                            MfCount = request.Reservation.Child
                        }
                    ],
                    RatePlans =
                    [
                        new RatePlan
                        {
                            reservationActionType = "NEW",
                            ratePlanRPH = 0,
                            ratePlanCode = request.Reservation.RoomStay.RateCode,
                            TimeSpan = new TimeUnit
                            {
                                TimeUnitType = "DAY",
                                StartTime = request.Reservation.DateIn,
                                NumberOfTimeUnits = request.Reservation.DateOut.Subtract(request.Reservation.DateIn).Days
                            },
                            mfsourceCode = request.Reservation.RoomStay.SourceCode,
                            mfMarketCode = request.Reservation.RoomStay.MarketCode,
                            Rates = rates
                        }
                    ],
                    resGuestRPHs = 0,
                    GuaranteeInfo = new GuaranteeInfo
                    {
                        GuaranteeType = "CCDCVOUCHER",
                        MfGuaranteeType = "GCO"
                    },
                    PaymentInstructions =
                    [
                        new PaymentInstruction
                        {
                            PaymentMethodType = "VOUCHER",
                            mfPaymentMethod = "CA",
                            PaymentDue = new PaymentDue
                            {
                                Amount = new Amount
                                {
                                    CurrencyCode = "RUB",
                                    ValueNum = request.Reservation.RoomStay.Amount
                                },
                                CancelIfNotReceived = 0
                            }
                        }
                    ],
                    mfsourceCode = request.Reservation.RoomStay.SourceCode,
                    marketSegmentCode = request.Reservation.RoomStay.MarketCode,
                    mfchannelCode = "B",
                    mfconfidentialRate = 0
                }
            ],
            MfupdateDate = DateTime.Now,
            MfComplementary = 0
        };

        XmlSerializerNamespaces ns = new XmlSerializerNamespaces([XmlQualifiedName.Empty]);

        XmlSerializer xmlSerializer = new XmlSerializer(typeof(Reservation));
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
                xmlSerializer.Serialize(writer, reservation, ns);
                xmlBody = sww.ToString();

                string xmlHeader = $"""
                                    <?xml version = '1.0'?>
                                    <?Label {request.Interfase.HotelCode}|RESERVATION|{request.Interfase.TransactionId}|{request.Interfase.Status}?>
                                    """;

                StringBuilder sb = new StringBuilder();
                sb.Append(xmlHeader);
                sb.Append('\n');
                sb.Append(xmlBody);

                var xml = sb.ToString();

                using (var httpClient = new HttpClient())
                {
                    httpClient.DefaultRequestHeaders.Clear();
                    httpClient.DefaultRequestHeaders.ConnectionClose = true;
                    httpClient.Timeout = TimeSpan.FromSeconds(10);

                    var queryParams = new Dictionary<string, string>
                    {
                        { "interfaceName", request.Interfase.InterfaseName },
                        { "propertyName", request.Interfase.HotelCode },
                        { "messageType", "RESERVATION" },
                        { "transactionId", request.Interfase.TransactionId },
                        { "status", request.Interfase.Status },
                    };

                    var queryBuilder = new QueryBuilder(queryParams);
                    string basePath = request.Interfase.Url;

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