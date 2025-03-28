using Common.HttpClientWrapper.Attributes;

namespace Kp.Ms.Oxi.Models.Request;

public class ShopPaymentReceiptContract
{
    [FormProperty("order_id")]
    public string OrderId { get; set; }

    [FormProperty("secret")]
    public string Secret { get; set; }
}
