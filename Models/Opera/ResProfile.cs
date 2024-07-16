using System.Xml.Serialization;

namespace Kp.Ms.Oxi.Models.Opera;

public class ResProfile
{
    [XmlElement("Profile", Namespace = "profile.fidelio.3.0")]
    public required Profile Profile { get; set; }

    [XmlElement("resProfileRPH")]
    public int? resProfileRPH { get; set; }
}