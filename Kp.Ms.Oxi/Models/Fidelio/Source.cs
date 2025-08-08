using System.Xml.Serialization;

namespace Kp.Ms.Oxi.Models.Fidelio;

[XmlType(TypeName = "Source")]
public class Source
{
    [XmlAttribute]
    public string AgentSine { get; set; }

    [XmlAttribute]
    public string AgentDutyCode { get; set; }

    [XmlElement]
    public RequestorID RequestorID { get; set; }
}
