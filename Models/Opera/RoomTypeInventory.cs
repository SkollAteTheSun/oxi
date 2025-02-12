using System.Xml.Serialization;

namespace Kp.Ms.Oxi.Models.Opera;
public class RoomTypeInventory
{
    [XmlAttribute("roomType")]
    public string RoomType { get; set; }

    [XmlAttribute("generic")]
    public int Generic { get; set; }

    [XmlAttribute("physicalRooms")]
    public int PhysicalRooms { get; set; }

    [XmlAttribute("roomTypeOverbook")]
    public int RoomTypeOverbook { get; set; }

    [XmlAttribute("outOfOrder")]
    public int OutOfOrder { get; set; }

    [XmlAttribute("virtualSuite")]
    public int VirtualSuite { get; set; }

    [XmlAttribute("available")]
    public int Available { get; set; }

    [XmlElement("Definite")]
    public InventoryDetail Definite { get; set; }

    [XmlElement("Tentative")]
    public InventoryDetail Tentative { get; set; }
}