namespace Kp.Ms.Oxi.Models.Entity;

public class RoomQuota
{
    public long Id { get; set; }
    public long HotelId { get; set; }
    public long RoomTypeId { get; set; }
    public DateTime Date { get; set; }
    public long Amount { get; set; }
    public long? RateId { get; set; }
    public bool Close { get; set; }
}