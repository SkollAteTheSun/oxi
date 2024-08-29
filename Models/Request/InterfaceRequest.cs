using System.ComponentModel.DataAnnotations;

namespace Kp.Ms.Oxi.Models.Request;

public class InterfaceRequest
{
    [Required]
    public required string Url { get; set; }

    [Required]
    public required string InterfaceName { get; set; }

    [Required]
    public required string TransactionId { get; set; }

    [Required]
    public required string Status { get; set; }

    [Required]
    public required string HotelCode { get; set; }

    public string? TravelAgentName { get; set; }

    public int? TravelAgentId { get; set; }

    public string? SourceName { get; set; }

    public int? SourceId { get; set; }
}