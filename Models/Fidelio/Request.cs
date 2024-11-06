using System.Xml.Serialization;

namespace Kp.Ms.Oxi.Models.Fidelio;

public class Request
{
    [XmlAttribute("ID")]
    public required string Id { get; set; }

    [XmlAttribute("UserName")]
    public required string UserName { get; set; }

    [XmlAttribute("Password")]
    public required string Password { get; set; }

    [XmlElement("insert")]
    public Insert? Insert { get; set; }
}