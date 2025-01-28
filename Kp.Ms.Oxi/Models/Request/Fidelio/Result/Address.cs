using System.Xml.Serialization;

namespace Kp.Ms.Oxi.Models.Request.Fidelio.Result;

public class Address
{
    [XmlAttribute("initID")]
    public string? InitId { get; set; }

    [XmlAttribute("newID")]
    public string? NewId { get; set; }
}