using System.Xml.Serialization;

namespace Kp.Ms.Oxi.Models.Opera;

public class ElectronicAddress
{
    [XmlAttribute("electronicAddressType")]
    public string? ElectronicAddressType { get; set; }

    [XmlElement("eAddress")]
    public required string Address { get; set; }

    [XmlElement("mfPrimaryYN")]
    public string? MfPrimary { get; set; }

    [XmlElement("confirmation")]
    public int Confirmation { get; set; }
}