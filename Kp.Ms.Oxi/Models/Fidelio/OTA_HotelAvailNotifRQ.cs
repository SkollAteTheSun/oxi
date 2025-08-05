using System.Xml.Serialization;

namespace Kp.Ms.Oxi.Models.Fidelio;


[XmlRoot(Namespace = "http://www.opentravel.org/OTA/2003/05")]
[XmlType(TypeName = "OTA_HotelAvailNotifRQ")]
public class OTA_HotelAvailNotifRQ
{
    [XmlElement]
    public POS POS { get; set; }

    [XmlElement]
    public AvailStatusMessages AvailStatusMessages { get; set; }
}
