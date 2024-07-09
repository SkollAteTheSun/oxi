using System.Xml.Serialization;

namespace Kp.Ms.Oxi.Models.Opera;

[XmlType(TypeName = "Profile")]
public class Profile
{
    [XmlAttribute("profileType")]
    public required string ProfileType { get; set; }

    [XmlAttribute("gender")]
    public required string Gender { get; set; }

    [XmlAttribute("multiProperty")]
    public int MultiProperty { get; set; }

    [XmlAttribute("miniProfile")]
    public int MiniProfile { get; set; }

    [XmlAttribute("pseudoProfile")]
    public int PseudoProfile { get; set; }

    [XmlAttribute("replaceAddress")]
    public int ReplaceAddress { get; set; }

    [XmlElement("profileID")]
    public ulong ProfileId { get; set; }

    [XmlElement("creatorCode")]
    public required string CreatorCode { get; set; }

    [XmlElement("createdDate")]
    public DateTime CreatedDate { get; set; }

    [XmlElement("lastUpdaterCode")]
    public required string LastUpdaterCode { get; set; }

    [XmlElement("lastUpdated")]
    public DateTime LastUpdated { get; set; }

    [XmlElement("genericName")]
    public required string GenericName { get; set; }

    [XmlElement("IndividualName")]
    public required IndividualName IndividualName { get; set; }

    [XmlElement("dateOfBirth")]
    public DateOnly DateOfBirth { get; set; }

    [XmlElement("primaryLanguageID")]
    public string? PrimaryLanguageId { get; set; }

    [XmlElement("mfResort")]
    public string? MfResort { get; set; }

    [XmlElement("mfResortProfileID")]
    public string? MfResortProfileId { get; set; }

    [XmlElement("mfNameCode")]
    public string? MfNameCode { get; set; }

    [XmlElement("mfAllowMail")]
    public string? MfAllowMail { get; set; }

    [XmlElement("mfAllowEMail")]
    public string? MfAllowEMail { get; set; }

    [XmlElement("mfGuestPriv")]
    public string? MfGuestPriv { get; set; }

    [XmlArray("ElectronicAddresses")]
    [XmlArrayItem("ElectronicAddress")]
    public List<ElectronicAddress>? ElectronicAddresses { get; set; }

    [XmlArray("PhoneNumbers")]
    [XmlArrayItem("PhoneNumber")]
    public List<PhoneNumber>? PhoneNumbers { get; set; }

    [XmlElement("mfAllowPhone")]
    public int? MfAllowPhone { get; set; }

    [XmlElement("mfAllowSMS")]
    public int? MfAllowSMS { get; set; }

    [XmlElement("PrivacyOption")]
    public PrivacyOption? PrivacyOption { get; set; }
}