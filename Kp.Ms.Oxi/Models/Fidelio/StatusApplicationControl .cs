using System.Xml.Serialization;

namespace Kp.Ms.Oxi.Models.Fidelio;

[XmlType(TypeName = "StatusApplicationControl")]
public class StatusApplicationControl
{
    [XmlAttribute]
    public DateTime Start { get; set; }

    [XmlAttribute]
    public DateTime End { get; set; }

    [XmlAttribute]
    public string RatePlanCode { get; set; }

    [XmlAttribute]
    public string InvTypeCode { get; set; }
}
