using System.Xml.Serialization;

namespace Kp.Ms.Oxi.Models.Opera;

public class PaymentInstruction
{
    [XmlAttribute("paymentMethodType")]
    public required string PaymentMethodType { get; set; }

    [XmlElement("mfPaymentMethod")]
    public required string mfPaymentMethod { get; set; }

    [XmlElement("PaymentDue")]
    public required PaymentDue PaymentDue { get; set; }
}