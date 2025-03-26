using System.Xml.Serialization;

namespace Kp.Ms.Oxi.Models.Opera;

public class RatePackages
{
    [XmlElement("RatePackage")]
    public List<RatePackage> RatePackage { get; set; }
}