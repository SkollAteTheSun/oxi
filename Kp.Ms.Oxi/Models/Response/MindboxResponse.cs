using Newtonsoft.Json;

namespace Kp.Ms.Oxi.Models.Response;

public class MindboxResponse
{
    [JsonProperty("success")]
    public bool Success { get; set; }

    [JsonProperty("error")]
    public int? Error { get; set; }

    [JsonProperty("errorMessage")]
    public string? ErrorMessage { get; set; }

    [JsonProperty("mindboxMessage")]
    public string? MindboxMessage { get; set; }

    [JsonProperty("data")]
    public object? Data { get; set; }
}
