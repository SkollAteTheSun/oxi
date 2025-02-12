using Newtonsoft.Json;

namespace Kp.Ms.Oxi.Models.Request;

public class Mail
{
    [JsonProperty("toEmail")]
    public string ToEmail { get; set; }

    [JsonProperty("parameters")]
    public MailParameters Parameters { get; set; }
}
