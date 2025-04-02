using System.Xml.Serialization;

namespace Kp.Ms.Oxi.Models.Opera;

public class RateDetails
{
    [XmlElement("RateDetail")]
    public List<RateDetail> RateDetail { get; set; }
}