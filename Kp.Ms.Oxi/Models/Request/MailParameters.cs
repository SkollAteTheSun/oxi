using Newtonsoft.Json;

namespace Kp.Ms.Oxi.Models.Request;

public class MailParameters
{
    [JsonProperty("content")]
    public string Content { get; set; }
}
