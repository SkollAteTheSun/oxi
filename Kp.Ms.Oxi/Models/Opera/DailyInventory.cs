using System.Xml.Serialization;

namespace Kp.Ms.Oxi.Models.Opera;

public class DailyInventory
{
    [XmlAttribute("datum")]
    public DateTime Datum { get; set; }

    [XmlAttribute("physicalRooms")]
    public int PhysicalRooms { get; set; }

    [XmlAttribute("houseOverbook")]
    public int HouseOverbook { get; set; }

    [XmlAttribute("outOfOrder")]
    public int OutOfOrder { get; set; }

    [XmlElement("Definite")]
    public InventoryDetail Definite { get; set; }

    [XmlArray("RoomTypeInventories")]
    [XmlArrayItem("RoomTypeInventory")]
    public List<RoomTypeInventory> RoomTypeInventories { get; set; }
}