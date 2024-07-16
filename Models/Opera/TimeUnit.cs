using System.Xml.Serialization;

namespace Kp.Ms.Oxi.Models.Opera;

public class TimeUnit
{
    [XmlAttribute("timeUnitType")]
    public required string TimeUnitType { get; set; }
    
    [XmlElement("startTime")]
    public DateTime StartTime { get; set; }

    [XmlElement("numberOfTimeUnits")]
    public int NumberOfTimeUnits { get; set; }
}