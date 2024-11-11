using System.Xml.Serialization;

namespace Kp.Ms.Oxi.Models.Request.Fidelio.Result;

public class Response
{
    [XmlAttribute("ID")]
    public string? Id { get; set; }

    [XmlAttribute("Status")]
    public string? Status { get; set; }

    [XmlAttribute("Message")]
    public string? Message { get; set; }

    [XmlElement("insertResponse")]
    public InsertResponse? InsertResponse { get; set; }

    [XmlElement("updateResponse")]
    public UpdateResponse? UpdateResponse { get; set; }

    [XmlElement("queryResponse")]
    public QueryResponse? QueryResponse { get; set; }
}