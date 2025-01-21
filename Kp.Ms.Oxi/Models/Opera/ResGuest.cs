using System.Xml.Serialization;

namespace Kp.Ms.Oxi.Models.Opera;

public class ResGuest
{
    [XmlAttribute("reservationActionType")]
    public required string ReservationActionType { get; set; }

    [XmlElement("resGuestRPH")]
    public int ResGuestRph { get; set; }

    [XmlElement("profileRPHs")]
    public required string ProfileRphs { get; set; }

    [XmlElement("InHouseTimeSpan")]
    public required TimeUnit InHouseTimeSpan { get; set; }
    
    [XmlElement("ArrivalTransport")]
    public required Transport ArrivalTransport { get; set; }
    
    [XmlElement("DepartureTransport")]
    public required Transport DepartureTransport { get; set; }

    [XmlElement("arrivalTime")]
    public required DateTime ArrivalTime { get; set; }

    [XmlElement("departureTime")]
    public required DateTime DepartureTime { get; set; }

    [XmlElement("reservationID")]
    public string? ReservationId { get; set; }

    [XmlElement("mfConfirmationLegNo")]
    public int MfConfirmationLegNo { get; set; }

    [XmlArray("ReservationReferences")]
    [XmlArrayItem("ReservationReference")]
    public List<ReservationReference> ReservationReferences { get; set; } = [];

    [XmlElement("preRegistered")]
    public int PreRegistered { get; set; }

    [XmlElement("commissionPaidTo")]
    public required string CommissionPaidTo { get; set; }
}