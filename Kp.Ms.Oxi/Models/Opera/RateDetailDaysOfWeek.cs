using System.Xml.Serialization;

namespace Kp.Ms.Oxi.Models.Opera;

public class RateDetailDaysOfWeek
{
    [XmlElement("monday")]
    public ushort? Monday { get; set; }

    [XmlElement("tuesday")]
    public ushort? Tuesday { get; set; }

    [XmlElement("wednesday")]
    public ushort? Wednesday { get; set; }

    [XmlElement("thursday")]
    public ushort? Thursday { get; set; }

    [XmlElement("friday")]
    public ushort? Friday { get; set; }

    [XmlElement("saturday")]
    public ushort? Saturday { get; set; }

    [XmlElement("sunday")]
    public ushort? Sunday { get; set; }
}