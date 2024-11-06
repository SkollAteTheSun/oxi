using System.Xml.Serialization;

namespace Kp.Ms.Oxi.Models.Fidelio;

[XmlRoot(Namespace = "V8XMLInterface")]
[XmlType(TypeName = "fidelio")]
public class Fidelio
{
    [XmlAttribute("Version")]
    public string Version { get; set; } = "1.0.0";

    [XmlElement("request")]
    public required Request Request { get; set; }
}