using System.Xml.Serialization;

namespace Kp.Ms.Oxi.Models.Fidelio;

[XmlType(TypeName = "POS")]
public class POS
{
    [XmlElement]
    public Source Source { get; set; }
}
