using System.Xml.Serialization;

namespace Kp.Ms.Oxi.Models.Opera;

public class RateCategory
{
    [XmlElement("catDescription")]
    public string Description { get; set; }

    [XmlElement("category")]
    public string Category { get; set; }
}