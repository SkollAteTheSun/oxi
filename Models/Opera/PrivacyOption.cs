using System.Xml.Serialization;

namespace Kp.Ms.Oxi.Models.Opera;

public class PrivacyOption
{
    [XmlElement("mfAllowMail")]
    public string? mfAllowMail { get; set; }

    [XmlElement("mfAllowEMail")]
    public string? mfAllowEMail { get; set; }

    [XmlElement("mfAllowPhone")]
    public int? mfAllowPhone { get; set; }

    [XmlElement("mfAllowSMS")]
    public int? mfAllowSMS { get; set; }

    [XmlElement("mfAllowHistory")]
    public int? mfAllowHistory { get; set; }

    [XmlElement("mfAllowMarketResearch")]
    public int? mfAllowMarketResearch { get; set; }

    [XmlElement("mfAllowThirdParty")]
    public int? mfAllowThirdParty { get; set; }
}