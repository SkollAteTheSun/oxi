using Newtonsoft.Json;

namespace Kp.Ms.Oxi.Models.Request;

public class ShopPaymentReceiptContract
{
    [JsonProperty("order_id")]
    public string OrderId { get; set; }

    [JsonProperty("secret")]
    public string Secret { get; set; }
}
