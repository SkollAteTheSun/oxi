using System.Xml.Serialization;

namespace Kp.Ms.Oxi.Models.Fidelio;

[XmlType(TypeName = "RateAmountMessages")]
public class RateAmountMessages
{
    [XmlAttribute]
    public string HotelCode { get; set; }

    [XmlElement("RateAmountMessage")]
    public List<RateAmountMessage> Messages { get; set; } = [];
}
