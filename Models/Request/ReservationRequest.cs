using System.ComponentModel.DataAnnotations;
using Kp.Ms.Oxi.Models.Enums;

namespace Kp.Ms.Oxi.Models.Request;

public class ReservationRequest
{
    public string? ExternalId { get; set; }

    [Required]
    public required string ReservationNumber { get; set; }

    [Required]
    public DateTime CreatedAt { get; set; }

    [Required]
    public DateTime DateIn { get; set; }

    [Required]
    public DateTime DateOut { get; set; }

    [Required]
    public required string RoomTypeCode { get; set; }

    [Required]
    public required int Adult { get; set; }

    [Required]
    public required int Child { get; set; }

    [Required]
    public required ProfileRequest Profile { get; set; }

    [Required]
    public required RoomStayRequest RoomStay { get; set; }

    [Required]
    public ReservationStatusType Status { get; set; }

    [Required]
    public float PaymentAmount { get; set; }

    public string? Comment { get; set; }
}