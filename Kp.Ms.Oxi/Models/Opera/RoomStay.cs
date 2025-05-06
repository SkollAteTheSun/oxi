using System.Xml.Serialization;

namespace Kp.Ms.Oxi.Models.Opera;

public class RoomStay
{
    [XmlAttribute("mfShareAction")]
    public required string mfShareAction { get; set; }

    [XmlAttribute("mfReservationAction")]
    public required string mfReservationAction { get; set; }

    [XmlAttribute("reservationActionType")]
    public required string reservationActionType { get; set; }

    [XmlAttribute("reservationStatusType")]
    public required string reservationStatusType { get; set; }

    [XmlElement("roomStayRPH")]
    public int? roomStayRPH { get; set; }

    [XmlElement("roomInventoryCode")]
    public required string roomInventoryCode { get; set; }

    [XmlElement("TimeSpan")]
    public required TimeUnit TimeSpan { get; set; }

    [XmlArray("GuestCounts")]
    [XmlArrayItem("GuestCount")]
    public List<GuestCount> GuestCounts { get; set; } = [];

    [XmlArray("RatePlans")]
    [XmlArrayItem("RatePlan")]
    public List<RatePlan> RatePlans { get; set; } = [];

    [XmlElement("resGuestRPHs")]
    public int? resGuestRPHs { get; set; }

    [XmlElement("GuaranteeInfo")]
    public GuaranteeInfo? GuaranteeInfo { get; set; }

    [XmlArray("PaymentInstructions")]
    [XmlArrayItem("PaymentInstruction")]
    public List<PaymentInstruction> PaymentInstructions { get; set; } = [];

    [XmlElement("mfsourceCode")]
    public required string mfsourceCode { get; set; }

    [XmlElement("marketSegmentCode")]
    public required string marketSegmentCode { get; set; }

    [XmlElement("mfchannelCode")]
    public string? mfchannelCode { get; set; }

    [XmlElement("mfconfidentialRate")]
    public int? mfconfidentialRate { get; set; }

    [XmlArray("InventoryItems")]
    [XmlArrayItem("InventoryItem")]
    public List<InventoryItem> InventoryItems { get; set; } = [];
}