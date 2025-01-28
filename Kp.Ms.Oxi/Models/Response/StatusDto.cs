using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Kp.Ms.Oxi.Models.Response;

public class StatusDto
{
    [Required]
    public required string Status { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Message { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Error { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? ErrorCode { get; set; }
}