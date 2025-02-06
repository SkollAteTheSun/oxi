using System.Xml.Serialization;

namespace Kp.Ms.Oxi.Models.Request.Fidelio.Result;

public class Row
{
    [XmlArray("fields")]
    [XmlArrayItem("field")]
    public List<Field> Fields { get; set; } = [];
}