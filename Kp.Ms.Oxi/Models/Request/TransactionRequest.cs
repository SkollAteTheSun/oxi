using System.ComponentModel.DataAnnotations;

namespace Kp.Ms.Oxi.Models.Request;

public class TransactionRequest
{
    [Required]
    public required string HotelCode { get; set; }

    [Required]
    public required string InterfaceName { get; set; }
}