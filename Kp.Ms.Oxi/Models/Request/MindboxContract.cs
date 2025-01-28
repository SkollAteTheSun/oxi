using Newtonsoft.Json;

namespace Kp.Ms.Oxi.Models.Request;

public class MindboxContract
{
    [JsonProperty("order_id")]
    public string OrderId { get; set; }

    [JsonProperty("status")]
    public int Status { get; set; }

    [JsonProperty("source")]
    public string Source { get; set; }
}
