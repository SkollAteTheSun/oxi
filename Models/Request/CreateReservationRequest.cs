using System.ComponentModel.DataAnnotations;

namespace Kp.Ms.Oxi.Models.Request;

public class CreateReservationRequest
{
    [Required]
    public required InterfaseRequest Interfase { get; set; }

    [Required]
    public required ReservationRequest Reservation { get; set; }
}