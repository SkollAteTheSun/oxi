using System.Xml.Serialization;

namespace Kp.Ms.Oxi.Models.Opera;

public class Transport
{
    [XmlElement("transportTime")]
    public required DateTime TransportTime { get; set; }
}