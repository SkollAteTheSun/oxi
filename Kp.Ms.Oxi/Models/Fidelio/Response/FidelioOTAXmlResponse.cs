using System.Xml.Serialization;

namespace Kp.Ms.Oxi.Models.Fidelio.Response;

public class FidelioOTAXmlResponse
{
    [XmlElement(IsNullable = false)]
    public string? Success { get; set; } = null;

    [XmlArray(IsNullable = false)]
    [XmlArrayItem("Warning")]
    public List<Warning>? Warnings { get; set; } = null;

    [XmlArray(IsNullable = false)]
    [XmlArrayItem("Error")]
    public List<Error>? Errors { get; set; } = null;
}
