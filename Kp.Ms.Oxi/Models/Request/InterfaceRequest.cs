using System.ComponentModel.DataAnnotations;
using Kp.Ms.Oxi.Models.Enums;

namespace Kp.Ms.Oxi.Models.Request;

public class InterfaceRequest
{
    public InterfaceType Type { get; set; } = InterfaceType.Oxi;

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

    [Required]
    public required string GuaranteeType { get; set; }

    [Required]
    public required string PaymentMethod { get; set; }

    public InterfaceAuthRequest? Auth { get; set; }

    public string? ChannelCode { get; set; }
}