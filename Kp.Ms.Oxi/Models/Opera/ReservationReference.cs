using System.Xml.Serialization;

namespace Kp.Ms.Oxi.Models.Opera;

public class ReservationReference
{
    [XmlAttribute("type")]
    public required string Type { get; set; }

    [XmlAttribute("referenceNumber")]
    public required string ReferenceNumber { get; set; }

    [XmlAttribute("legNumber")]
    public int LegNumber { get; set; }
}