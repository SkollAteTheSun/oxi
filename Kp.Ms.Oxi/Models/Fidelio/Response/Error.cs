using System.Xml.Serialization;

namespace Kp.Ms.Oxi.Models.Fidelio.Response;

[XmlType(TypeName = "Error")]
public class Error
{
    [XmlAttribute]
    public string Type { get; set; }

    [XmlAttribute]
    public string Status { get; set; }

    [XmlAttribute]
    public string ShortText { get; set; }

    [XmlText]
    public string Text { get; set; }
}
