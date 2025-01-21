using System.Xml.Serialization;

namespace Kp.Ms.Oxi.Models.Opera;

public class RatePlan
{
    [XmlAttribute("reservationActionType")]
    public required string reservationActionType { get; set; }

    [XmlElement("ratePlanRPH")]
    public int? ratePlanRPH { get; set; }

    [XmlElement("ratePlanCode")]
    public required string ratePlanCode { get; set; }

    [XmlElement("TimeSpan")]
    public required TimeUnit TimeSpan { get; set; }
    
    [XmlElement("mfsourceCode")]
    public required string mfsourceCode { get; set; }

    [XmlElement("mfMarketCode")]
    public required string mfMarketCode { get; set; }

    [XmlArray("Rates")]
    [XmlArrayItem("Rate")]
    public List<Rate> Rates { get; set; } = [];
}