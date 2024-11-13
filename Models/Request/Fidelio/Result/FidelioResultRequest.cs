using System.Xml.Serialization;

namespace Kp.Ms.Oxi.Models.Request.Fidelio.Result;

[XmlRoot("fidelio")]
public class FidelioResultRequest
{
    [XmlAttribute("Version")]
    public string? Version { get; set; }

    [XmlElement("response")]
    public required Response Response { get; set; }
}