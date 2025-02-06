using System.Xml.Serialization;

namespace Kp.Ms.Oxi.Models.Opera;

public class UdfDefinition
{
    [XmlAttribute("module")]
    public string? Module { get; set; }

    [XmlElement("pmsTableName")]
    public required string PmsTableName { get; set; }

    [XmlElement("pmsColumnName")]
    public required string PmsColumnName { get; set; }
}