using System.Xml.Serialization;

namespace Kp.Ms.Oxi.Models.Opera;

public class IndividualName
{
    [XmlElement("nameFirst")]
    public required string NameFirst { get; set; }

    [XmlElement("nameSur")]
    public required string NameSur { get; set; }
}