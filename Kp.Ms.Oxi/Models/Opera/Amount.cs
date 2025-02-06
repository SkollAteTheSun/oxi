using System.Xml.Serialization;

namespace Kp.Ms.Oxi.Models.Opera;

public class Amount
{
    [XmlAttribute("currencyCode")]
    public required string CurrencyCode { get; set; }

    [XmlElement("valueNum", IsNullable = true)]
    public double? ValueNum { get; set; }
}