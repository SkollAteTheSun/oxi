using System.Xml.Serialization;

namespace Kp.Ms.Oxi.Models.Opera;

[XmlRoot("RtavMessage", Namespace = "rtav.fidelio.4.0")]
public class RtavMessage
{
    [XmlAttribute("dataMart")]
    public int DataMart { get; set; }

    [XmlElement("HotelReference")]
    public HotelReference HotelReference { get; set; }

    [XmlArray("DailyInventories")]
    [XmlArrayItem("DailyInventory")]
    public List<DailyInventory> DailyInventories { get; set; }
}