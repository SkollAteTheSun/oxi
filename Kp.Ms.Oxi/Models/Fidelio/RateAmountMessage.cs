using System.Xml.Serialization;

namespace Kp.Ms.Oxi.Models.Fidelio;

[XmlType(TypeName = "RateAmountMessage")]
public class RateAmountMessage
{
    [XmlElement]
    public StatusApplicationControl StatusApplicationControl { get; set; }

    [XmlArray]
    [XmlArrayItem("Rate")]
    public List<FidelioRate> Rates { get; set; } = [];
}
