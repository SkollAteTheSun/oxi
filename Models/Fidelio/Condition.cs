using System.Xml.Serialization;

namespace Kp.Ms.Oxi.Models.Fidelio;

public class Condition
{
    [XmlAttribute("name")]
    public required string Name { get; set; }

    [XmlText]
    public string? Value { get; set; }

    [XmlAttribute("operation")]
    public required string Operation { get; set; }
}