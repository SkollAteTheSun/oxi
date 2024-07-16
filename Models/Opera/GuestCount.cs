using System.Xml.Serialization;

namespace Kp.Ms.Oxi.Models.Opera;

[XmlType(TypeName = "GuestCount")]
public class GuestCount
{
    [XmlElement("ageQualifyingCode")]
    public required string AgeQualifyingCode { get; set; }

    [XmlElement("mfCount")]
    public int MfCount { get; set; }
}