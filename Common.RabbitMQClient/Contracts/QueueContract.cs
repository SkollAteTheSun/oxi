using Newtonsoft.Json;

namespace Common.RabbitMQClient.Contracts;

public class QueueContract
{
    [JsonProperty("name")]
    public string Name { get; set; }

    [JsonProperty("messages")]
    public int Messages { get; set; }

    [JsonProperty("consumers")]
    public int Consumers { get; set; }

    [JsonProperty("node")]
    public string Node { get; set; }

    [JsonProperty("state")]
    public string State { get; set; }

    [JsonProperty("type")]
    public string Type { get; set; }
}
