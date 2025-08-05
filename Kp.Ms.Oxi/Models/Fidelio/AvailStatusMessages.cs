using System.Xml.Serialization;

namespace Kp.Ms.Oxi.Models.Fidelio;

[XmlType(TypeName = "AvailStatusMessages")]
public class AvailStatusMessages
{
    [XmlAttribute]
    public string HotelCode { get; set; }

    [XmlElement("AvailStatusMessage")]
    public List<AvailStatusMessage> Messages { get; set; } = [];
}
