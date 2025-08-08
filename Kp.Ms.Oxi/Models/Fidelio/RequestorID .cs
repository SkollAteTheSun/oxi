using System.Xml.Serialization;

namespace Kp.Ms.Oxi.Models.Fidelio;

[XmlType(TypeName = "RequestorID")]
public class RequestorID
{
    [XmlAttribute]
    public string URL { get; set; }

    [XmlAttribute]
    public string Type { get; set; }

    [XmlAttribute]
    public string ID { get; set; }
}
