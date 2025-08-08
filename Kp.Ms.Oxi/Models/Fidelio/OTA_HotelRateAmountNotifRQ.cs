using System.Xml.Serialization;

namespace Kp.Ms.Oxi.Models.Fidelio;

[XmlRoot(Namespace = "http://www.opentravel.org/OTA/2003/05")]
[XmlType(TypeName = "OTA_HotelRateAmountNotifRQ")]
public class OTA_HotelRateAmountNotifRQ
{
    [XmlElement]
    public POS POS { get; set; }

    [XmlElement]
    public RateAmountMessages RateAmountMessages { get; set; }
}
