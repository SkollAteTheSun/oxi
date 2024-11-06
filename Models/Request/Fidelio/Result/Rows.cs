using System.Xml.Serialization;

namespace Kp.Ms.Oxi.Models.Request.Fidelio.Result;

public class Rows
{
    [XmlElement("ProfileID")]
    public string? ProfileId { get; set; }

    [XmlElement("GuestNum")]
    public int? GuestNum { get; set; }

    [XmlArray("Addresses")]
    [XmlArrayItem("Address")]
    public List<Address> Addresses { get; set; } = [];

    [XmlArray("Communications")]
    [XmlArrayItem("Communication")]
    public List<Communication>? Communications { get; set; } = [];
}