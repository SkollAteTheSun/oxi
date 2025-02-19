namespace Kp.Ms.Oxi.Models.Request;

public class CheckOutInfo
{
    public string Id { get; set; }
    public string? ExternalId { get; set; }
    public string? BookingId { get; set; }
    public DateTime CheckOutDate { get; set; }
    public string HotelCode { get; set; }
}
