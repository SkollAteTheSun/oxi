using System.Xml.Serialization;

namespace Kp.Ms.Oxi.Models.Opera;

public class RatePackage
{
    [XmlElement("Package", Namespace = "packages.fidelio.3.0")]
    public Package Package { get; set; }
}