using System.Xml.Serialization;
using Kp.Ms.Oxi.Interfaces;

namespace Kp.Ms.Oxi.Models.Fidelio;

[XmlRoot(Namespace = "V8XMLInterface")]
[XmlType(TypeName = "fidelio")]
public class Fidelio : IReservationContent
{
    [XmlAttribute("Version")]
    public string Version { get; set; } = "1.0.0";

    [XmlElement("request")]
    public required Request Request { get; set; }
}