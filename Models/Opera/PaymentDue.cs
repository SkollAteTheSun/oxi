using System.Xml.Serialization;

namespace Kp.Ms.Oxi.Models.Opera;

public class PaymentDue
{
    [XmlElement("Amount")]
    public required Amount Amount { get; set; }

    [XmlElement("cancelIfNotReceived")]
    public int CancelIfNotReceived { get; set; }
}