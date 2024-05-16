using System.ComponentModel.DataAnnotations;

namespace Kp.Ms.Oxi.Models.Request;

public class InterfaseRequest
{
    [Required]
    public required string Url { get; set; }
    
    [Required]
    public required string InterfaseName { get; set; }
    
    [Required]
    public required string PropertyName { get; set; }
    
    [Required]
    public required string TransactionId { get; set; }
    
    [Required]
    public required string Status { get; set; }
    
    [Required]
    public required string HotelCode { get; set; }
}