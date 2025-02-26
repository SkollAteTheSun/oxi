using System.Xml.Serialization;

namespace Kp.Ms.Oxi.Models.Opera;

[XmlType(TypeName = "HotelReference")]
public class HotelReferenceAttribute
{
    [XmlAttribute("hotelCode")]
    public required string HotelCode { get; set; }
}
