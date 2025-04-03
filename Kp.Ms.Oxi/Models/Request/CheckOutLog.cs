namespace Kp.Ms.Oxi.Models.Request;

public class CheckOutLog
{
    public string PmsId { get; set; }
    public string ExternalId { get; set; }
    public string? BookingId { get; set; }
    public DateTime CheckOutDate { get; set; }
    public string HotelCode { get; set; }
    public DateTime Timestamp { get; set; }
    public string Status { get; set; }
    public string Description { get; set; }

    public CheckOutLog()
    {
        Timestamp = DateTime.UtcNow;
    }
}
