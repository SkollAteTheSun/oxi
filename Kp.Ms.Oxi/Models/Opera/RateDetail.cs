using System.Xml.Serialization;

namespace Kp.Ms.Oxi.Models.Opera;

public class RateDetail
{
    [XmlElement("rateCode")]
    public required string RateCode { get; set; }
    
    [XmlElement("startDate")]
    public required string DateStart { get; set; }
    
    [XmlElement("endDate")]
    public required string DateEnd { get; set; }
    
    [XmlElement("roomTypeList")]
    public List<string> RoomTypes { get; set; } = [];
    
    [XmlElement("marketCode")]
    public required string MarketCode { get; set; }
    
    [XmlElement("rate1")]
    public ulong Rate1 { get; set; }
    
    [XmlElement("rate2")]
    public ulong? Rate2 { get; set; }
    
    [XmlElement("rate3")]
    public ulong? Rate3 { get; set; }
    
    [XmlElement("rate4")]
    public ulong? Rate4 { get; set; }
    
    [XmlElement("rate5")]
    public ulong? Rate5 { get; set; }
    
    [XmlElement("child1RateParentRoom")]
    public ulong? Child1RateParentRoom { get; set; }
    
    [XmlElement("child2RateParentRoom")]
    public ulong? Child2RateParentRoom { get; set; }
    
    [XmlElement("child3RateParentRoom")]
    public ulong? Child3RateParentRoom { get; set; }
    
    [XmlElement("child1RateOwnRoom")]
    public ulong? Child1RateOwnRoom { get; set; }
    
    [XmlElement("child2RateOwnRoom")]
    public ulong? Child2RateOwnRoom { get; set; }
    
    [XmlElement("child3RateOwnRoom")]
    public ulong? Child3RateOwnRoom { get; set; }
    
    [XmlElement("child4RateOwnRoom")]
    public ulong? Child4RateOwnRoom { get; set; }
    
    [XmlElement("extraBed")]
    public ulong ExtraBed { get; set; }
    
    [XmlElement("RateDaysOfWeek")]
    public required RateDetailDaysOfWeek DaysOfWeek { get; set; }
    
    [XmlElement("payingChildren")]
    public int PayingChildren { get; set; }
    
    [XmlElement("freeChildren")]
    public int FreeChildren { get; set; }
}