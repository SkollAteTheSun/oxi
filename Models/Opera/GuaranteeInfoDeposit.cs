using System.Xml.Serialization;

namespace Kp.Ms.Oxi.Models.Opera;

public class GuaranteeInfoDeposit
{
    [XmlElement("Amount")]
    public required Amount Amount { get; set; }

    [XmlElement("cancelIfNotReceived")]
    public double CancelIfNotReceived { get; set; }
}