namespace Kp.Ms.Oxi.Models.Response;

public class CheckOutResult
{
    public bool Success { get; set; }
    public string ErrorMessage { get; set; }
    public string HotelCode { get; set; }
    public string ExternalId { get; set; }
}
