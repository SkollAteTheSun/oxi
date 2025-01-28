using System.Xml.Serialization;

namespace Kp.Ms.Oxi.Models.Fidelio;

public class Command
{
    [XmlElement("cancel")]
    public Cancel? Cancel { get; set; }
}