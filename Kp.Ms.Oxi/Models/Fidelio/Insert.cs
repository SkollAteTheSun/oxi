using System.Xml.Serialization;

namespace Kp.Ms.Oxi.Models.Fidelio;

public class Insert
{
    [XmlAttribute("ReqType")]
    public required string ReqType { get; set; }

    [XmlAttribute("TranslationTable")]
    public required string TranslationTable { get; set; }

    [XmlArray("fields")]
    [XmlArrayItem("field")]
    public List<Field> Fields { get; set; } = [];
}