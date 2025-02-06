using System.Xml.Serialization;

namespace Kp.Ms.Oxi.Models.Opera;

public class Udf
{
    [XmlElement("UdfDefinition", Namespace = "udfdefinition.fidelio.2.0")]
    public required UdfDefinition UdfDefinition { get; set; }

    [XmlElement("udfValue")]
    public string? UdfValue { get; set; }
}