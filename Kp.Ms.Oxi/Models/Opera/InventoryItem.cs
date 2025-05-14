using System.Xml.Serialization;

namespace Kp.Ms.Oxi.Models.Opera;

public class InventoryItem
{
    [XmlAttribute("type")]
    public required string Type { get; set; }

    [XmlElement("item")]
    public required string Item { get; set; }

    [XmlElement("quantity")]
    public int Quantity { get; set; }

    [XmlElement("beginDate")]
    public required string BeginDate { get; set; }

    [XmlElement("endDate")]
    public required string EndDate { get; set; }

    [XmlElement("beginDateTime")]
    public DateTime BeginDateTime { get; set; }

    [XmlElement("endDateTime")]
    public DateTime EndDateTime { get; set; }
}