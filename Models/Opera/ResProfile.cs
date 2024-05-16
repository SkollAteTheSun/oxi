using System.Xml.Serialization;

namespace Kp.Ms.Oxi.Models.Opera;

public class ResProfile
{
    [XmlElement("Profile")]
    public required Profile Profile { get; set; }

    [XmlElement("resProfileRPH")]
    public int? resProfileRPH { get; set; }
}