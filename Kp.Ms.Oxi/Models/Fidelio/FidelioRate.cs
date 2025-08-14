using System.Xml.Serialization;

namespace Kp.Ms.Oxi.Models.Fidelio;

[XmlType(TypeName = "Rate")]
public class FidelioRate
{
    [XmlAttribute]
    public DateTime Start { get; set; }

    [XmlAttribute]
    public DateTime End { get; set; }

    [XmlAttribute]
    public bool Mon { get; set; }

    [XmlAttribute]
    public bool Tue { get; set; }

    [XmlAttribute]
    public bool Weds { get; set; }

    [XmlAttribute]
    public bool Thur { get; set; }

    [XmlAttribute]
    public bool Fri { get; set; }

    [XmlAttribute]
    public bool Sat { get; set; }

    [XmlAttribute]
    public bool Sun { get; set; }

    [XmlArray]
    [XmlArrayItem("BaseByGuestAmt")]
    public List<BaseByGuestAmt> BaseByGuestAmts { get; set; } = [];

    [XmlArray]
    [XmlArrayItem("AdditionalGuestAmount")]
    public List<AdditionalGuestAmount> AdditionalGuestAmounts { get; set; } = [];
}
