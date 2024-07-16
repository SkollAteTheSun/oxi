using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Kp.Ms.Oxi.Models.Request;

public class TransactionGetResponseRequest
{
    [Required]
    [JsonPropertyName("api_token")]
    public required string Token { get; set; }

    [Required]
    [JsonPropertyName("transaction_id")]
    public required string TransactionId { get; set; }

    [Required]
    [JsonPropertyName("status")]
    public required string Status { get; set; }

    [Required]
    [JsonPropertyName("response")]
    public required string Response { get; set; }

    [JsonPropertyName("reservation_external_id")]
    public string? ReservationExternalId { get; set; }

    [JsonPropertyName("guest_external_id")]
    public string? GuestExternalId { get; set; }
}