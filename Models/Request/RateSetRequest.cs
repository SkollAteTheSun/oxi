using System.ComponentModel.DataAnnotations;

namespace Kp.Ms.Oxi.Models.Request;

public class RateSetRequest
{
    [Required]
    public long TransactionId { get; set; }
    
    [Required]
    public long HotelId { get; set; }
    
    [Required]
    public long RateId { get; set; }
    
    [Required]
    public DateOnly DateStart { get; set; }
    
    [Required]
    public DateOnly DateEnd { get; set; }
    
    [Required]
    public List<long> RoomTypeIds { get; set; } = [];
}