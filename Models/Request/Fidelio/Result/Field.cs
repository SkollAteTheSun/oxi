using System.Xml.Serialization;

namespace Kp.Ms.Oxi.Models.Request.Fidelio.Result;

public class Field
{
    [XmlAttribute("name")]
    public required string Name { get; set; }

    [XmlText]
    public string? Value { get; set; }
}