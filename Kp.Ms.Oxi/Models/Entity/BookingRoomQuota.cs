namespace Kp.Ms.Oxi.Models.Entity;

public class BookingRoomQuota
{
    public ulong Id { get; set; }

    public ulong HotelId { get; set; }

    public ulong RoomTypeId { get; set; }

    public DateTime Date { get; set; }

    public long? Amount { get; set; }

    public ulong? RateId { get; set; }

    public bool? Close { get; set; } = false;

    public long AmountAsu { get; set; }

    public long ReservedAsu { get; set; }
}