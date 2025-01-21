using System.Xml.Serialization;

namespace Kp.Ms.Oxi.Models.Request.Fidelio.Result;

public class UpdateResponse
{
    [XmlElement("rows")]
    public required Rows Rows { get; set; }
}