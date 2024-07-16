using System.ComponentModel.DataAnnotations;

namespace Kp.Ms.Oxi.Models.Request;

public class RoomStayRequest
{
    [Required]
    public required string RateCode { get; set; }

    [Required]
    public required string MarketCode { get; set; }

    [Required]
    public required string SourceCode { get; set; }

    [Required]
    public List<RoomStayRateRequest> Rates { get; set; } = [];
    
    [Required]
    public double Amount { get; set; }
}