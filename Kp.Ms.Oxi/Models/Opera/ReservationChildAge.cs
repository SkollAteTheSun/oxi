using System.Xml.Serialization;

namespace Kp.Ms.Oxi.Models.Opera;

public class ReservationChildAge
{
    [XmlAttribute("childAge")]
    public int ChildAge { get; set; }
}