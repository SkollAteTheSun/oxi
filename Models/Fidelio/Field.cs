using System.Xml.Serialization;

namespace Kp.Ms.Oxi.Models.Fidelio;

public class Field
{
    [XmlAttribute("name")]
    public required string Name { get; set; }

    [XmlText]
    public string? Value { get; set; }

    [XmlAttribute("attr")]
    public string? Attr { get; set; }

    [XmlAttribute("Primary")]
    public string? Primary { get; set; }

    [XmlAttribute("description")]
    public string? Description { get; set; }
}