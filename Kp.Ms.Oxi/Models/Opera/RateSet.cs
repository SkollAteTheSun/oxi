using System.Xml.Serialization;

namespace Kp.Ms.Oxi.Models.Opera;

[XmlRoot(Namespace = "rate.fidelio.6.0")]
[XmlType(TypeName = "RateHeader")]
public class RateSet
{
    [XmlAttribute("rateStatus")]
    public required string RateStatus { get; set; }
    
    [XmlAttribute("yieldStatus")]
    public required string YieldStatus { get; set; }
    
    [XmlAttribute("rateMessageType")]
    public required string RateMessageType { get; set; }
    
    [XmlElement("hotelCode")]
    public required string HotelCode { get; set; }

    [XmlElement("rateCode")]
    public required string RateCode { get; set; }

    [XmlArray("RateDetails")]
    [XmlArrayItem("RateDetail")]
    public List<RateDetail> RateDetails { get; set; } = [];

    [XmlElement("dbaseFltPct")]
    public required string DbaseFltPct { get; set; }

    [XmlElement("dbaseRounding")]
    public required string DbaseRounding { get; set; }

    [XmlElement("dbaseCompare")]
    public int DbaseCompare { get; set; }

    [XmlElement("advBaseRounding")]
    public required string AdvBaseRounding { get; set; }
}