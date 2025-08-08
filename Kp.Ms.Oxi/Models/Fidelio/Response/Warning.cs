using System.Xml.Serialization;

namespace Kp.Ms.Oxi.Models.Fidelio.Response;

[XmlType(TypeName = "Warning")]
public class Warning
{
    [XmlAttribute]
    public string Type { get; set; }

    [XmlAttribute]
    public string Language { get; set; }

    [XmlAttribute]
    public string Code { get; set; }

    [XmlAttribute]
    public string Tag { get; set; }

    [XmlText]
    public string Text { get; set; }
}
