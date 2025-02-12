using System.Xml.Serialization;

namespace Kp.Ms.Oxi.Models.Request.Fidelio.Result;

public class QueryResponse
{
    [XmlElement("rows")]
    public required Rows Rows { get; set; }
}