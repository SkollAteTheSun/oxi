using System.Xml.Serialization;

namespace Kp.Ms.Oxi.Models.Opera;

public class GuaranteeInfo
{
    [XmlAttribute("guaranteeType")]
    public required string GuaranteeType { get; set; }

    [XmlElement("mfGuaranteeType")]
    public required string MfGuaranteeType { get; set; }
}