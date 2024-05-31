using System.ComponentModel.DataAnnotations;

namespace Kp.Ms.Oxi.Models.Request;

public class CreateReservationRequest
{
    [Required]
    public required InterfaceRequest Interface { get; set; }

    [Required]
    public required ReservationRequest Reservation { get; set; }
}