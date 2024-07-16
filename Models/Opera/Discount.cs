using System.Xml.Serialization;

namespace Kp.Ms.Oxi.Models.Opera;

public class Discount
{
    [XmlAttribute("type")]
    public required string Type { get; set; }

    [XmlElement("valueNum")]
    public double ValueNum { get; set; }
}