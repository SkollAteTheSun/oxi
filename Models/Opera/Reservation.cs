using System.Xml.Serialization;

namespace Kp.Ms.Oxi.Models.Opera;

[XmlRoot(Namespace = "reservation.fidelio.4.0")]
[XmlType(TypeName = "Reservation")]
public class Reservation
{
    [XmlAttribute("mfShareAction")]
    public required string MfShareAction { get; set; }
    
    [XmlAttribute("mfReservationAction")]
    public required string MfReservationAction { get; set; }

    [XmlElement("HotelReference")]
    public required HotelReference HotelReference { get; set; }

    [XmlElement("reservationID")]
    public string? ReservationId { get; set; }

    [XmlElement("reservationOriginatorCode")]
    public required string ReservationOriginatorCode { get; set; }

    [XmlElement("mfLegNumbers")]
    public int MfLegNumbers { get; set; }

    [XmlElement("originalBookingDate")]
    public DateTime OriginalBookingDate { get; set; }
    
    [XmlElement("StayDateRange")]
    public required TimeUnit StayDateRange { get; set; }

    [XmlArray("GuestCounts")]
    [XmlArrayItem("GuestCount")]
    public List<GuestCount> GuestCounts { get; set; } = [];

    [XmlArray("ResComments")]
    [XmlArrayItem("ResComment")]
    public List<ResComment> ResComments { get; set; } = [];

    [XmlArray("ResGuests")]
    [XmlArrayItem("ResGuest")]
    public List<ResGuest> ResGuests { get; set; } = [];

    [XmlArray("ResProfiles")]
    [XmlArrayItem("ResProfile")]
    public List<ResProfile> ResProfiles { get; set; } = [];

    [XmlArray("RoomStays")]
    [XmlArrayItem("RoomStay")]
    public List<RoomStay> RoomStays { get; set; } = [];
    
    [XmlElement("mfupdateDate")]
    public DateTime MfupdateDate { get; set; }

    [XmlElement("mfcomplementary")]
    public int? MfComplementary { get; set; }
}