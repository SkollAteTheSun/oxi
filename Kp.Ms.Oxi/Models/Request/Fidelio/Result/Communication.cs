using System.Xml.Serialization;

namespace Kp.Ms.Oxi.Models.Request.Fidelio.Result;

public class Communication
{
    [XmlAttribute("newID")]
    public string? NewId { get; set; }
}