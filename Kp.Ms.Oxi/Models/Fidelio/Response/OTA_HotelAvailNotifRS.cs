using System.Xml.Serialization;

namespace Kp.Ms.Oxi.Models.Fidelio.Response;

[XmlRoot(Namespace = "http://www.opentravel.org/OTA/2003/05")]
[XmlType(TypeName = "OTA_HotelAvailNotifRS")]
public class OTA_HotelAvailNotifRS : FidelioOTAXmlResponse
{
}
