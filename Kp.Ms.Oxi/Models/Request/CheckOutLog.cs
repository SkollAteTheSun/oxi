using Kp.Ms.Oxi.Models.Enums;

namespace Kp.Ms.Oxi.Models.Request;

public class CheckOutLog
{
    public string Id { get; set; }
    public string ExternalId { get; set; }
    public string? BookinglId { get; set; }
    public DateTime CheckOutDate { get; set; }
    public string HotelCode { get; set; }
    public DateTime Timestamp { get; set; }
    public string Status { get; set; }

    public CheckOutLog()
    {
        Timestamp = DateTime.UtcNow;
    }
}
