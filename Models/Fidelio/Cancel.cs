using System.Xml.Serialization;

namespace Kp.Ms.Oxi.Models.Fidelio;

public class Cancel
{
    [XmlElement("GuestNum")]
    public required string GuestNum { get; set; }

    [XmlElement("Reason")]
    public string? Reason { get; set; }
}