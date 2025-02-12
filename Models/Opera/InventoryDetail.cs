using System.Xml.Serialization;

namespace Kp.Ms.Oxi.Models.Opera;

public class InventoryDetail
{
    [XmlAttribute("count")]
    public int Count { get; set; }

    [XmlAttribute("blockCount")]
    public int BlockCount { get; set; }

    [XmlAttribute("resvCount")]
    public int ResvCount { get; set; }
}
