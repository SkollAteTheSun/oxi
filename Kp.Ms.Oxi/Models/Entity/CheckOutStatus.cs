using Kp.Ms.Oxi.Models.Enums;

namespace Kp.Ms.Oxi.Models.Entity;

public class CheckOutStatus
{
    public Guid Id { get; set; }
    public string BookingId { get; set; }
    public CheckOutProcessingStatus Status { get; set; }
    public DateTime Date { get; set; }
}
