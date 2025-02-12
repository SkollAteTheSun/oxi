namespace Kp.Ms.Oxi.Models.Entity;

public class RoomRatesVector
{
    public string? RateCode { get; set; }
    public long RateId { get; set; }
    public string? RoomTypeCode { get; set; }
    public long RoomTypeId { get; set; }
    public DateOnly DateStart { get; set; }
    public DateOnly DateEnd { get; set; }
    public ushort Adults { get; set; }
    public ushort Children { get; set; }
    public ushort Babies { get; set; }
    public ulong Amount { get; set; }
    public ushort ExtraBed { get; set; }
    public ushort Craddle { get; set; }
    public ulong BasePrice { get; set; }
    public ulong BabyPrice { get; set; }
    public ulong CraddlePrice { get; set; }
    public ulong ChildrenPrice { get; set; }
    public ulong BabyPriceOnBeds { get; set; }
    public ulong AdultPriceOnBeds { get; set; }
    public ulong ChildrenPriceOnBeds { get; set; }
}