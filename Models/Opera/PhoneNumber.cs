using System.Xml.Serialization;

namespace Kp.Ms.Oxi.Models.Opera;

public class PhoneNumber
{
    [XmlAttribute("phoneNumberType")]
    public string? PhoneNumberType { get; set; }

    [XmlElement("phoneNumber")]
    public required string Number { get; set; }

    [XmlElement("mfPrimaryYN")]
    public string? MfPrimary { get; set; }

    [XmlElement("confirmation")]
    public int Confirmation { get; set; }
}