using System.Xml.Serialization;

namespace Kp.Ms.Oxi.Models.Fidelio;

[XmlType(TypeName = "BaseByGuestAmt")]
public class BaseByGuestAmt
{
    [XmlAttribute]
    public int NumberOfGuests { get; set; }

    [XmlAttribute]
    public int AgeQualifyingCode { get; set; }

    [XmlAttribute]
    public ulong AmountAfterTax { get; set; }

    [XmlAttribute]
    public string CurrencyCode { get; set; }
}
