using System.Xml.Serialization;

namespace Kp.Ms.Oxi.Models.Opera;

public class ResComment
{
    [XmlAttribute("reservationActionType")]
    public required string ReservationActionType { get; set; }

    [XmlElement("resCommentRPH")]
    public int? ResCommentRPH { get; set; }

    [XmlElement("commentOriginatorCode")]
    public required string СommentOriginatorCode { get; set; }

    [XmlElement("guestViewable")]
    public int? GuestViewable { get; set; }

    [XmlElement("comment")]
    public required string Comment { get; set; }

    [XmlElement("mfcommentType")]
    public required string MfcommentType { get; set; }

    [XmlElement("mfcommentDate")]
    public DateTime MfcommentDate { get; set; }
}