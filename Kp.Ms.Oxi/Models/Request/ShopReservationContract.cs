using Newtonsoft.Json;

namespace Kp.Ms.Oxi.Models.Request;

public class ShopReservationContract
{
    [JsonProperty("key")]
    public string Key { get; set; }

    [JsonProperty("reservation_number")]
    public string Number { get; set; }

    [JsonProperty("reservation_id")]
    public string Id { get; set; }

    [JsonProperty("reservation_status")]
    public string Status { get; set; } 
}
