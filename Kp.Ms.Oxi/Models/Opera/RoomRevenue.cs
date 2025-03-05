using System.Xml.Serialization;

namespace Kp.Ms.Oxi.Models.Opera;

public class RoomRevenue
{
    [XmlElement("roomStayRPH")]
    public DateTime StayDate { get; set; }

    [XmlElement("NetRoomRate")]
    public NetRoomRate NetRoomRate { get; set; }
}
