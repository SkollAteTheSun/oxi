using System.ComponentModel.DataAnnotations;

namespace Kp.Ms.Oxi.Models.Request;

public class InterfaсeRequest
{
    [Required]
    public required string Url { get; set; }
    
    [Required]
    public required string InterfaсeName { get; set; }
    
    [Required]
    public required string TransactionId { get; set; }
    
    [Required]
    public required string Status { get; set; }
    
    [Required]
    public required string HotelCode { get; set; }
}