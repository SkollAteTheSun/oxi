using System.ComponentModel.DataAnnotations;

namespace Kp.Ms.Oxi.Models.Request;

public class RoomStayRateRequest
{
    [Required]
    public DateTime Date { get; set; }

    [Required]
    public double Amount { get; set; }

    [Required]
    public double Discount { get; set; }
}