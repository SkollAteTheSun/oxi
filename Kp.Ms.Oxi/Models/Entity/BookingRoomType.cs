namespace Kp.Ms.Oxi.Models.Entity;

public class BookingRoomType
{
    public ulong Id { get; set; }

    public string Name { get; set; } = null!;

    public string Code { get; set; } = null!;

    public byte Capacity { get; set; }

    public int CountMainSeat { get; set; }

    public string? Description { get; set; }

    public string? Images { get; set; }

    public ulong HotelId { get; set; }

    public bool? Active { get; set; }

    public string? Data { get; set; }

    public DateTime? DeletedAt { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public byte Order { get; set; }

    public string? AccommodationTypes { get; set; }

    public int? MinNightsRoomType { get; set; }

    public uint? ResourceId { get; set; }

    public ushort? SquareFrom { get; set; }

    public ushort? SquareTo { get; set; }

    public sbyte? ExtrabedCapacity { get; set; }

    public bool NotAvailForChildren { get; set; }

    public bool NotAvailForBaby { get; set; }

    public string? HrsCode { get; set; }

    public sbyte? ChildrenCapacity { get; set; }

    public sbyte? CradleCapacity { get; set; }

    public string? Panorama { get; set; }
}
