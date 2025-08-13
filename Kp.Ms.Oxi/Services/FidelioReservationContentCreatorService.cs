using System.Text;
using System.Text.Json;
using Kp.Ms.Oxi.Common;
using Kp.Ms.Oxi.Interfaces;
using Kp.Ms.Oxi.Models.Fidelio;
using Kp.Ms.Oxi.Models.Request;

namespace Kp.Ms.Oxi.Services;

public class FidelioReservationContentCreatorService : IReservationContentCreator
{
    private readonly IConfiguration _configuration;

    public FidelioReservationContentCreatorService(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    public IReservationContent GetReservationContent(SendReservationRequest request)
    {
        Fidelio fidelio = new Fidelio
        {
            Request = new Request
            {
                Id = request.Interface.TransactionId,
                UserName = request.Interface.Auth?.UserName,
                Password = GetDecryptedPassword(request),
                Insert = new Insert
                {
                    ReqType = string.IsNullOrWhiteSpace(request.Reservation.Profile.ExternalId) ? "ProfileAndReservation" : "Reservation",
                    TranslationTable = "XML",
                    Fields = BuildFields(request)
                }
            }
        };

        return fidelio;
    }

    public string GetXmlHeader(SendReservationRequest request)
    {
        return $"""
                <?xml version="1.0" encoding="UTF-8"?>
                """;
    }

    private List<Field> BuildFields(SendReservationRequest request)
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
            new Field { Name = "CurrencyCode", Value = "RUR" },
            new Field { Name = "CompanyID", Value = request.Interface.TravelAgentId.ToString() },
            new Field { Name = "ReservationAttribute", Attr = "KUR", Description = "Payer", Value = "0" },
            new Field { Name = "NoOfAdults", Value = request.Reservation.Adult.ToString() },
            new Field { Name = "Notes", Attr = "RMA", Value = request.Reservation.Comment },
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
                new Field { Name = "ProfileCategory", Value = "4" },    // за это поле не уверен, надо норм потестить
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
                new Field { Name = "ProfileID", Value = request.Reservation.Profile.ExternalId }
            });
        }

        return fields;
    }

    private string GetDecryptedPassword(SendReservationRequest request)
    {
        byte[] bytes = Convert.FromBase64String(request.Interface.Auth.Password);
        byte[] key = Encoding.ASCII.GetBytes(_configuration["AesEncryption:Key"] ?? throw new Exception("Aes key is not set"));
        byte[] iv = Encoding.ASCII.GetBytes(request.Interface.Auth.Iv);
        
        return AesEncryption.Decrypt(bytes, key, iv);
    }
}