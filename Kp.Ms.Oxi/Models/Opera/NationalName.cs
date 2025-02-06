using System.Xml.Serialization;

namespace Kp.Ms.Oxi.Models.Opera;

public class NationalName
{
    [XmlElement("nameFirst")]
    public required string NameFirst { get; set; }

    [XmlElement("nameLast")]
    public required string NameLast { get; set; }
}