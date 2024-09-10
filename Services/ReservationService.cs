using System.Globalization;
using System.Text;
using System.Text.Json;
using System.Xml;
using System.Xml.Serialization;
using Kp.Ms.Oxi.Common;
using Kp.Ms.Oxi.Context;
using Kp.Ms.Oxi.Models.Entity;
using Kp.Ms.Oxi.Models.Enums;
using Kp.Ms.Oxi.Models.Opera;
using Kp.Ms.Oxi.Models.Request;
using Microsoft.AspNetCore.Http.Extensions;

namespace Kp.Ms.Oxi.Services;

public class ReservationService
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<ReservationService> _logger;
    private readonly DefaultContext _context;

    public ReservationService(IConfiguration configuration, ILogger<ReservationService> logger, DefaultContext context)
    {
        _configuration = configuration;
        _logger = logger;
        _context = context;
    }

    public async Task<string> Send(SendReservationRequest request)
    {
        MakeTransactionId(request);

        string reservationActionType = request.Reservation.ExternalId == null ? "NEW" : "CHANGE";
        string mfReservationAction = request.Reservation.ExternalId == null ? "ADD" : "EDIT";
        string reservationStatusType = request.Reservation.Status switch
        {
            ReservationStatusType.New or ReservationStatusType.Paid or ReservationStatusType.Approved => "RESERVED",
            ReservationStatusType.Cancelled => "CANCELLED",
            _ => "RESERVED"
        };

        List<Rate> rates = [];
        int rateNum = 0;
        rates.AddRange(request.Reservation.RoomStay.Rates.Select(rate => new Rate
        {
            reservationActionType = reservationActionType,
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
            MfReservationAction = mfReservationAction,
            HotelReference = new HotelReference
            {
                HotelCode = request.Interface.HotelCode
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
                    ReservationActionType = reservationActionType,
                    ResCommentRPH = 0,
                    СommentOriginatorCode = "SYSTEM",
                    GuestViewable = 1,
                    Comment = "RATE_FIXED=NO",
                    MfcommentType = "SYSTEM",
                    MfcommentDate = request.Reservation.CreatedAt
                },
                new ResComment
                {
                    ReservationActionType = reservationActionType,
                    ResCommentRPH = 1,
                    СommentOriginatorCode = "SYSTEM",
                    GuestViewable = 1,
                    Comment = "DISCOUNT_PERCENTAGE=0",
                    MfcommentType = "SYSTEM",
                    MfcommentDate = request.Reservation.CreatedAt
                },
                new ResComment
                {
                    ReservationActionType = reservationActionType,
                    ResCommentRPH = 2,
                    СommentOriginatorCode = "SYSTEM",
                    GuestViewable = 1,
                    Comment = "PARTY=",
                    MfcommentType = "SYSTEM",
                    MfcommentDate = request.Reservation.CreatedAt
                },
                new ResComment
                {
                    ReservationActionType = reservationActionType,
                    ResCommentRPH = 3,
                    СommentOriginatorCode = "SYSTEM",
                    GuestViewable = 1,
                    Comment = $"HOTEL_DATE={request.Reservation.DateIn}",
                    MfcommentType = "SYSTEM",
                    MfcommentDate = request.Reservation.CreatedAt
                },
                new ResComment
                {
                    ReservationActionType = reservationActionType,
                    ResCommentRPH = 4,
                    СommentOriginatorCode = "SUPERVISOR",
                    GuestViewable = 1,
                    Comment = MakeComment(request),
                    MfcommentType = "RESERVATION",
                    MfcommentDate = request.Reservation.CreatedAt
                }
            ],
            ResGuests =
            [
                new ResGuest
                {
                    ReservationActionType = reservationActionType,
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
            ResProfiles = MakeProfiles(request),
            RoomStays =
            [
                new RoomStay
                {
                    mfShareAction = "NA",
                    mfReservationAction = mfReservationAction,
                    reservationActionType = reservationActionType,
                    reservationStatusType = reservationStatusType,
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
                            reservationActionType = reservationActionType,
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
                        GuaranteeType = "NA",
                        MfGuaranteeType = "7",
                        GuaranteeDeposit = new GuaranteeInfoDeposit
                        {
                            Amount = new Amount
                            {
                                CurrencyCode = "RUB",
                                ValueNum = 0
                            },
                            CancelIfNotReceived = 0
                        }
                    },
                    PaymentInstructions =
                    [
                        new PaymentInstruction
                        {
                            PaymentMethodType = "NA",
                            mfPaymentMethod = "CA",
                            PaymentDue = new PaymentDue
                            {
                                Amount = new Amount
                                {
                                    CurrencyCode = "RUB",
                                    ValueNum = 0 //request.Reservation.RoomStay.Amount
                                },
                                CancelIfNotReceived = 0
                            }
                        }
                    ],
                    mfsourceCode = request.Reservation.RoomStay.SourceCode,
                    marketSegmentCode = request.Reservation.RoomStay.MarketCode,
                    mfchannelCode = "P",
                    mfconfidentialRate = 0
                }
            ],

            /*
             * Module = "RESERVATION" PmsTableName = "RESERVATION_NAME"
             * "UDFC35" // Resort fee
             * "UDFC33" // GDS Record Loc.
             * "UDFC30" // Ref Dossier
             * "UDFC22" // Розовая плашка под Source
             * "UDFC16" // Auto Code
             * "UDFC15" // PAY ID
             * "UDFC14" // Payment Type
             * "UDFC13" // Currency
             * "UDFC12" // Alias
             * "UDFC10" // TARS Tariff Name
             * "UDFC09" // TARS Tariff Code
             * "UDFC06" // Telephone
             * "UDFC05" // Reserve Par
             * "UDFC04" // Voucher #
             * "UDFC02" // Par
             * "UDFC01" // Raison RTC
             */
            Udfs = [
                new Udf
                {
                    UdfDefinition = new UdfDefinition
                    {
                        Module = "RESERVATION",
                        PmsTableName = "RESERVATION_NAME",
                        PmsColumnName = "UDFC35" // Resort fee
                    },
                    UdfValue = "00"
                },
                new Udf
                {
                    UdfDefinition = new UdfDefinition
                    {
                        Module = "RESERVATION",
                        PmsTableName = "RESERVATION_NAME",
                        PmsColumnName = "UDFC04" // Voucher #
                    },
                    UdfValue = request.Reservation.ReservationNumber
                }
            ],

            MfupdateDate = DateTime.UtcNow,
            MfComplementary = 0
        };

        XmlSerializerNamespaces ns = new XmlSerializerNamespaces([XmlQualifiedName.Empty]);

        XmlSerializer xmlSerializer = new XmlSerializer(typeof(Reservation));
        string xmlBody = "";

        XmlWriterSettings settings = new XmlWriterSettings
        {
            OmitXmlDeclaration = true
        };

        string xml;
        using (var sww = new StringWriter())
        {
            using (XmlWriter writer = XmlWriter.Create(sww, settings))
            {
                xmlSerializer.Serialize(writer, reservation, ns);
                xmlBody = sww.ToString();

                string xmlHeader = $"""
                                    <?xml version = '1.0'?>
                                    <?Label {request.Interface.HotelCode}|RESERVATION|{request.Interface.TransactionId}|{request.Interface.Status}?>
                                    """;

                StringBuilder sb = new StringBuilder();
                sb.Append(xmlHeader);
                sb.Append('\n');
                sb.Append(xmlBody);

                xml = sb.ToString();
            }
        }

        string? apiResponse;

        var queryParams = new Dictionary<string, string>
        {
            { "interfaceName", request.Interface.InterfaceName },
            { "propertyName", request.Interface.HotelCode },
            { "messageType", "RESERVATION" },
            { "transactionId", request.Interface.TransactionId },
            { "status", request.Interface.Status },
        };
        var queryBuilder = new QueryBuilder(queryParams);
        string basePath = request.Interface.Url;

        bool directMode = Convert.ToBoolean(_configuration["DirectMode"]);
        if (directMode)
        {
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Clear();
                httpClient.DefaultRequestHeaders.ConnectionClose = true;
                httpClient.Timeout = TimeSpan.FromSeconds(10);

                var requestMessage = new HttpRequestMessage(HttpMethod.Post, basePath + queryBuilder);
                requestMessage.Content = new StringContent(xml, Encoding.UTF8, "text/xml");
                using (var response = httpClient.SendAsync(requestMessage).Result)
                {
                    response.EnsureSuccessStatusCode();
                    apiResponse = response.Content.ReadAsStringAsync().Result;
                }
            }
        }
        else
        {
            _context.Transactions.Add(new Transaction
            {
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                HotelCode = request.Interface.HotelCode,
                TransactionId = request.Interface.TransactionId,
                InterfaceName = request.Interface.InterfaceName,
                Status = request.Interface.Status,
                Request = JsonSerializer.Serialize(request),
                Url = basePath + queryBuilder,
                Xml = xml
            });
            await _context.SaveChangesAsync();

            apiResponse = "saved to database";
        }

        return apiResponse;
    }

    private static string MakeComment(SendReservationRequest request)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("tot: ");
        sb.Append(request.Reservation.RoomStay.Amount.ToString("0.00"));
        sb.Append('\n');
        sb.Append("paid: ");
        sb.Append(request.Reservation.PaymentAmount.ToString("0.00"));
        if (!string.IsNullOrEmpty(request.Reservation.Comment))
        {
            sb.Append('\n');
            sb.Append(request.Reservation.Comment);
        }

        return sb.ToString();
    }

    private static void MakeTransactionId(SendReservationRequest request)
    {
        request.Interface.TransactionId = "BK" + request.Interface.TransactionId;
    }

    private static List<ResProfile> MakeProfiles(SendReservationRequest request)
    {
        List<ResProfile> profiles = [];

        ResProfile contact = new ResProfile
        {
            Profile = new Profile
            {
                ProfileType = ProfileType.Contact.ToString().ToUpper(),
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
                GenericName = CyrillicToLatin.Convert(request.Reservation.Profile.LastName),
                PrimaryLanguageId = "E",
                IndividualName = new IndividualName
                {
                    NameFirst = CyrillicToLatin.Convert(request.Reservation.Profile.FirstName),
                    NameSur = CyrillicToLatin.Convert(request.Reservation.Profile.LastName),
                },
                AlternateLanguageIDs = "R",
                MfNationalName = new NationalName
                {
                    NameFirst = request.Reservation.Profile.FirstName,
                    NameLast = request.Reservation.Profile.LastName,
                },
                DateOfBirth = request.Reservation.Profile.DateOfBirth,
                MfResort = request.Interface.HotelCode,
                MfResortProfileId = request.Reservation.Profile.ExternalId,
                MfAllowMail = "YES",
                MfAllowEMail = "NO",
                MfGuestPriv = "NO",
                ElectronicAddresses =
                [
                    new ElectronicAddress
                    {
                        ElectronicAddressType = "EMAIL",
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
        };
        profiles.Add(contact);

        if (request.Interface is { TravelAgentName: not null, TravelAgentId: not null })
        {
            ResProfile travelAgent = new ResProfile
            {
                Profile = new Profile
                {
                    ProfileType = ProfileType.Travel.ToString().ToUpper(),
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
                    GenericName = request.Interface.TravelAgentName,
                    IndividualName = new IndividualName
                    {
                        NameFirst = request.Interface.TravelAgentName,
                        NameSur = ""
                    },
                    MfResortProfileId = request.Interface.TravelAgentId.ToString(),
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
                resProfileRPH = profiles.Count
            };
            profiles.Add(travelAgent);
        }

        if (request.Interface is { SourceName: not null, SourceId: not null })
        {
            ResProfile source = new ResProfile
            {
                Profile = new Profile
                {
                    ProfileType = ProfileType.Wholesaler.ToString().ToUpper(),
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
                    GenericName = request.Interface.SourceName,
                    IndividualName = new IndividualName
                    {
                        NameFirst = request.Interface.SourceName,
                        NameSur = ""
                    },
                    MfResortProfileId = request.Interface.SourceId.ToString(),
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
                resProfileRPH = profiles.Count
            };
            profiles.Add(source);
        }

        return profiles;
    }
}