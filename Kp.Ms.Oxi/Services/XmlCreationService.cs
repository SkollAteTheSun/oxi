using System.Text;
using System.Xml;
using System.Xml.Serialization;
using Kp.Ms.Oxi.Models.Enums;
using Kp.Ms.Oxi.Models.Fidelio;
using Kp.Ms.Oxi.Models.Opera;
using Kp.Ms.Oxi.Models.Request;
using Kp.Ms.Oxi.Common;
using Kp.Ms.Oxi.Models.Options;
using Microsoft.Extensions.Options;

namespace Kp.Ms.Oxi.Services;

public class XmlCreationService
{
    private readonly IConfiguration _config;
    private readonly ReservationTaxOptions _taxOptions;

    public XmlCreationService(IConfiguration config, IOptions<ReservationTaxOptions> taxOptions)
    {
        _config = config;
        _taxOptions = taxOptions.Value;
    }
    
    #region General

    private void MakeTransactionId(SendReservationRequest request)
    {
        request.Interface.TransactionId = "BK" + request.Interface.TransactionId;
    }
    
    private string MakeComment(SendReservationRequest request)
    {
        StringBuilder sb = new StringBuilder();

        sb.Append("tot: ");
        sb.Append(request.Reservation.RoomStay.Amount.ToString("0.00"));
        sb.Append('\n');
        sb.Append("paid: ");
        sb.Append(request.Reservation.PaymentAmount.ToString("0.00"));

        if (request.Reservation.ChildrenAges.Count > 0)
        {
            sb.Append('\n');
            sb.AppendJoin(" / ", request.Reservation.ChildrenAges.Select(childrenAge => $"реб {childrenAge} лет").ToList());
        }

        if (!string.IsNullOrEmpty(request.Reservation.Comment))
        {
            sb.Append('\n');
            sb.Append(request.Reservation.Comment);
        }

        return sb.ToString();
    }

    private string CreateXml<TRequest>(TRequest obj, SendReservationRequest request)  // тут я бы унаследовал все обхекты Fidelio, Oxi и т.д. от одного базового объекта с общими свойствами и указал бы, что TRequest обязан быть наследником базового класса
    {
        // 🔧 Сериализация XML
        XmlSerializerNamespaces ns = new XmlSerializerNamespaces([XmlQualifiedName.Empty]);
        XmlSerializer xmlSerializer = new XmlSerializer(typeof(TRequest));
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
                xmlSerializer.Serialize(writer, obj, ns);
                xmlBody = sww.ToString();

                string xmlHeader = GetXmlHeader(obj, request);

                StringBuilder sb = new StringBuilder();
                sb.Append(xmlHeader);
                sb.Append('\n');
                sb.Append(xmlBody);

                xml = sb.ToString();
            }
        }

        return xml;
    }

    private string GetXmlHeader<TRequest>(TRequest obj, SendReservationRequest request)
    {
        return obj switch
        {
            Fidelio => $"""
                            <?xml version="1.0" encoding="UTF-8"?>
                            """,
            Reservation => $"""
                                    <?xml version = '1.0'?>
                                    <?Label {request.Interface.HotelCode}|RESERVATION|{request.Interface.TransactionId}|{request.Interface.Status}?>
                                    """
        };
    }

    #endregion

    #region Fidelio

    public string CreateFidelioReservationXml(SendReservationRequest request)
    {
        MakeTransactionId(request);
        
        byte[] bytes = Convert.FromBase64String(request.Interface.Auth.Password);
        byte[] key = Encoding.ASCII.GetBytes(_config["AesEncryption:Key"] ?? throw new Exception("Aes key is not set"));
        byte[] iv = Encoding.ASCII.GetBytes(request.Interface.Auth.Iv);
        string decryptedPassword = AesEncryption.Decrypt(bytes, key, iv);
        
        Fidelio fidelio = new Fidelio
        {
            Request = new Request
            {
                Id = request.Interface.TransactionId,
                UserName = request.Interface.Auth.UserName,
                Password = decryptedPassword,
                Insert = new Insert
                {
                    ReqType = string.IsNullOrWhiteSpace(request.Reservation.Profile.ExternalId) ? "ProfileAndReservation" : "Reservation",
                    TranslationTable = "XML",
                    Fields = BuildFidelioFields(request)
                }
            }
        };

        return CreateXml(fidelio, request);
    }

    private List<Field> BuildFidelioFields(SendReservationRequest request)
    {
        var fields = new List<Field>
        {
            new Field { Name = "RoomType", Value = request.Reservation.RoomTypeCode },
            new Field { Name = "RateRoomType", Value = request.Reservation.RoomTypeCode },
            new Field { Name = "GuestArrival", Value = request.Reservation.DateIn.ToString("dd.MM.yyyy") },
            new Field { Name = "GuestDeparture", Value = request.Reservation.DateOut.ToString("dd.MM.yyyy") },
            new Field { Name = "ReservationStatus", Value = "1" },
            new Field { Name = "ReservationType", Value = "RES" },
            new Field { Name = "MarketCode", Value = request.Reservation.RoomStay.MarketCode },
            new Field { Name = "SourceCode", Value = request.Reservation.RoomStay.SourceCode },
            new Field { Name = "ChannelCode", Value = request.Interface.ChannelCode },
            new Field { Name = "RateCode", Value = request.Reservation.RoomStay.RateCode },
            new Field { Name = "CurrencyCode", Value = "RUB" },
            new Field { Name = "CompanyID", Value = request.Interface.TravelAgentId.ToString() },
            new Field { Name = "ReservationAttribute", Attr = "KUR", Description = "Payer", Value = "0" },
            new Field { Name = "NoOfAdults", Value = request.Reservation.Adult.ToString() },
            new Field { Name = "Notes", Attr = "RMA", Value = MakeComment(request) },
            new Field { Name = "AllowRoomOverbooking", Value = "1" },
            new Field { Name = "CrsResNumber", Value = request.Reservation.ReservationNumber }
        };

        // По дням
        fields.AddRange(request.Reservation.RoomStay.Rates.Select(rate => new Field
        {
            Name = "RateValue",
            Value = rate.Amount.ToString("0.00"),
            StartDate = rate.Date.ToString("dd.MM.yyyy"),
            EndDate = rate.Date.AddDays(1).ToString("dd.MM.yyyy")
        }));
        
        // Дети
        fields.AddRange(request.Reservation.ChildAgeCats
            .Where(cac => cac.Count > 0)
            .Select(cac => new Field
            {
                Name = "Child",
                Attr = cac.Code,
                Value = cac.Count.ToString()
            }));

        if (string.IsNullOrEmpty(request.Reservation.Profile.ExternalId))
        {
            fields.AddRange(new List<Field>
            {
                new Field { Name = "ProfileType", Value = "2" },
                new Field { Name = "GuestName", Value = request.Reservation.Profile.LastName },
                new Field { Name = "GuestFirstname", Value = request.Reservation.Profile.FirstName },
                new Field { Name = "Birthday", Value = request.Reservation.Profile.DateOfBirth.ToString("dd.MM.yyyy") },
                new Field { Name = "Communication", Attr = "MPH", Primary = "1", Value = request.Reservation.Profile.Phone },
                new Field { Name = "Communication", Attr = "EAD", Primary = "1", Value = request.Reservation.Profile.Email },
                new Field { Name = "Language", Value = "RUS" },
                new Field { Name = "CountryISO3", Value = "RUS" },
            });
        }
        else
        {
            fields.AddRange(new List<Field>
            {
                new Field { Name = "ProfileID", Value = request.Reservation.Profile.ExternalId },
                new Field { Name = "ProfileCategory", Value = "4" }    // за это поле не уверен, надо норм потестить
            });
        }

        return fields;
    }

    #endregion

    #region Oxi

    public string CreateOxiReservationXml(SendReservationRequest request)
    {
        MakeTransactionId(request);

        string reservationActionType = request.Reservation.ExternalId == null ? "NEW" : "CHANGE";
        ReservationAction mfReservationAction = request.Reservation.ExternalId == null ? ReservationAction.ADD : ReservationAction.EDIT;
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
            MfReservationAction = mfReservationAction.ToString(),
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
                    AgeQualifyingCode = ReservationGuestAgeCode.ADULT.ToString(),
                    MfCount = request.Reservation.Adult
                },

                new GuestCount
                {
                    AgeQualifyingCode = ReservationGuestAgeCode.CHILD.ToString(),
                    MfCount = request.Reservation.Child
                }
            ],
            ResComments =
            [
                // new ResComment
                // {
                //     ReservationActionType = reservationActionType,
                //     ResCommentRPH = 0,
                //     СommentOriginatorCode = "SYSTEM",
                //     GuestViewable = 1,
                //     Comment = "RATE_FIXED=NO",
                //     MfcommentType = "SYSTEM",
                //     MfcommentDate = request.Reservation.CreatedAt
                // },
                // new ResComment
                // {
                //     ReservationActionType = reservationActionType,
                //     ResCommentRPH = 1,
                //     СommentOriginatorCode = "SYSTEM",
                //     GuestViewable = 1,
                //     Comment = "DISCOUNT_PERCENTAGE=0",
                //     MfcommentType = "SYSTEM",
                //     MfcommentDate = request.Reservation.CreatedAt
                // },
                // new ResComment
                // {
                //     ReservationActionType = reservationActionType,
                //     ResCommentRPH = 2,
                //     СommentOriginatorCode = "SYSTEM",
                //     GuestViewable = 1,
                //     Comment = "PARTY=",
                //     MfcommentType = "SYSTEM",
                //     MfcommentDate = request.Reservation.CreatedAt
                // },
                // new ResComment
                // {
                //     ReservationActionType = reservationActionType,
                //     ResCommentRPH = 3,
                //     СommentOriginatorCode = "SYSTEM",
                //     GuestViewable = 1,
                //     Comment = $"HOTEL_DATE={request.Reservation.DateIn}",
                //     MfcommentType = "SYSTEM",
                //     MfcommentDate = request.Reservation.CreatedAt
                // },
                new ResComment
                {
                    ReservationActionType = reservationActionType,
                    ResCommentRPH = 0,
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
                    CommissionPaidTo = "N",
                    ReservationChildAges = request.Reservation.ChildrenAges.Select(childAge => new ReservationChildAge
                    {
                        ChildAge = childAge
                    }).ToList()
                }
            ],
            ResProfiles = MakeProfiles(request),
            RoomStays =
            [
                new RoomStay
                {
                    mfShareAction = "NA",
                    mfReservationAction = mfReservationAction.ToString(),
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
                            AgeQualifyingCode = ReservationGuestAgeCode.ADULT.ToString(),
                            MfCount = request.Reservation.Adult
                        },
                        new GuestCount
                        {
                            AgeQualifyingCode = ReservationGuestAgeCode.CHILD.ToString(),
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
                        MfGuaranteeType = request.Interface.GuaranteeType,
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
                            mfPaymentMethod = request.Interface.PaymentMethod,
                            PaymentDue = new PaymentDue
                            {
                                Amount = new Amount
                                {
                                    CurrencyCode = "RUB",
                                    ValueNum = 0
                                },
                                CancelIfNotReceived = 0
                            }
                        }
                    ],
                    mfsourceCode = request.Reservation.RoomStay.SourceCode,
                    marketSegmentCode = request.Reservation.RoomStay.MarketCode,
                    mfchannelCode = "P",
                    mfconfidentialRate = 0,
                    InventoryItems = request.Reservation.ExtraBed > 0 && !string.IsNullOrEmpty(request.Interface.ExtraBedInventoryCode)
                        ? [
                            new InventoryItem
                            {
                                Type = "Housekeeping",
                                Item = request.Interface.ExtraBedInventoryCode,
                                Quantity = 1,
                                BeginDate = DateOnly.FromDateTime(request.Reservation.DateIn).ToString("yyyy-MM-dd"),
                                EndDate = DateOnly.FromDateTime(request.Reservation.DateOut).ToString("yyyy-MM-dd"),
                                BeginDateTime = new DateTime(
                                    request.Reservation.DateIn.Year,
                                    request.Reservation.DateIn.Month,
                                    request.Reservation.DateIn.Day,
                                    0,
                                    0,
                                    0
                                ),
                                EndDateTime = new DateTime(
                                    request.Reservation.DateOut.Year,
                                    request.Reservation.DateOut.Month,
                                    request.Reservation.DateOut.Day,
                                    23,
                                    59,
                                    59
                                )
                            }
                        ]
                        : []
                }
            ],
            Udfs =
            [
                new Udf
                {
                    UdfDefinition = new UdfDefinition
                    {
                        Module = "RESERVATION",
                        PmsTableName = "RESERVATION_NAME",
                        PmsColumnName = string.IsNullOrEmpty(request.Interface.TouristTaxMapping)
                        ? _taxOptions.TouristTaxMapping
                        : request.Interface.TouristTaxMapping // Resort fee
                    },
                    UdfValue = string.IsNullOrEmpty(request.Interface.TouristTaxValue)
                    ? _taxOptions.TouristTaxValue
                    : request.Interface.TouristTaxValue
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

        return CreateXml(reservation, request);
    }

    private List<ResProfile> MakeProfiles(SendReservationRequest request)
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
                },
                MfVipStatus = request.Reservation.Profile.LoyaltyStatus
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

    #endregion
}