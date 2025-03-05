using System.Xml.Serialization;

namespace Kp.Ms.Oxi.Models.Opera;

public class NetRoomRate
{
    [XmlAttribute("currencyCode")]
    public string CurrencyCode { get; set; }

    [XmlElement("valueNum")]
    public int ValueNum { get; set; }
}
