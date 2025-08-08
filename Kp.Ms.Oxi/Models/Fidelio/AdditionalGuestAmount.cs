using System.Xml.Serialization;

namespace Kp.Ms.Oxi.Models.Fidelio;

[XmlType(TypeName = "AdditionalGuestAmount")]
public class AdditionalGuestAmount
{
    [XmlAttribute]
    public int AgeQualifyingCode { get; set; }

    [XmlAttribute]
    public ulong Amount { get; set; }

    [XmlAttribute]
    public string CurrencyCode { get; set; }

    [XmlAttribute]
    public bool TaxInclusive { get; set; }
}
