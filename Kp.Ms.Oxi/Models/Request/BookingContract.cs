using Newtonsoft.Json;

namespace Kp.Ms.Oxi.Models.Request;

public class BookingContract
{
    [JsonProperty("reservation_id")]
    public string ReservationId { get; set; }

    [JsonProperty("date_check_out")]
    public DateTime DateCheckOut { get; set; }

    [JsonProperty("api_token")]
    public string ApiToken { get; set; }
}
