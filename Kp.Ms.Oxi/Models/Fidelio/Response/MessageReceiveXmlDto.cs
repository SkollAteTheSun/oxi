using System.Xml.Serialization;

namespace Kp.Ms.Oxi.Models.Fidelio.Response;

[XmlRoot]
public class MessageReceiveXmlDto
{
    [XmlElement("response")]
    public required string Response { get; set; }
}