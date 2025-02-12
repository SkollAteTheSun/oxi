using System.Xml.Serialization;

namespace Kp.Ms.Oxi.Models.Fidelio;

public class Update
{
    [XmlAttribute("ReqType")]
    public required string ReqType { get; set; }

    [XmlArray("conditions")]
    [XmlArrayItem("condition")]
    public List<Condition> Conditions { get; set; } = [];

    [XmlArray("fields")]
    [XmlArrayItem("field")]
    public List<Field> Fields { get; set; } = [];
}