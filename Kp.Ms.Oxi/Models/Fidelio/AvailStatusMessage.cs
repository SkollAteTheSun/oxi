using System.Xml.Serialization;

namespace Kp.Ms.Oxi.Models.Fidelio;

[XmlType(TypeName = "AvailStatusMessage")]
public class AvailStatusMessage
{
    [XmlAttribute]
    public int BookingLimit { get; set; }

    [XmlElement]
    public StatusApplicationControl StatusApplicationControl { get; set; }
}
