using System.Xml.Serialization;

namespace Kp.Ms.Oxi.Models.Opera;

public class Rate
{
    [XmlAttribute("reservationActionType")]
    public required string reservationActionType { get; set; }

    [XmlAttribute("rateBasisTimeUnitType")]
    public required string rateBasisTimeUnitType { get; set; }

    [XmlElement("rateRPH")]
    public int? rateRPH { get; set; }

    [XmlElement("Amount")]
    public required Amount Amount { get; set; }

    [XmlElement("rateBasisUnits")]
    public int? rateBasisUnits { get; set; }

    [XmlElement("TimeSpan")]
    public required TimeUnit TimeSpan { get; set; }

    [XmlElement("mfAdults")]
    public int? mfAdults { get; set; }

    [XmlElement("mfChildren")]
    public int? mfChildren { get; set; }

    [XmlElement("mfsourceCode")]
    public string? mfsourceCode { get; set; }

    [XmlElement("mfMarketCode")]
    public string? mfMarketCode { get; set; }

    [XmlElement("Discount")]
    public required Discount Discount { get; set; }

    [XmlElement("rateRoomType")]
    public required string rateRoomType { get; set; }

    [XmlElement("mfChildren1")]
    public int? mfChildren1 { get; set; }

    [XmlElement("mfChildren2")]
    public int? mfChildren2 { get; set; }

    [XmlElement("mfChildren3")]
    public int? mfChildren3 { get; set; }

    [XmlElement("mfChildren4")]
    public int? mfChildren4 { get; set; }

    [XmlElement("mfChildren5")]
    public int? mfChildren5 { get; set; }
}