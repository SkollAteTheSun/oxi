namespace Kp.Ms.Oxi.Models.Entity;

public class RoomRatesVectorAgg
{
    public required string RateCode { get; set; }
    public long RateId { get; set; }
    public required string RoomTypeCode { get; set; }
    public long RoomTypeId { get; set; }
    public DateOnly DateStart { get; set; }
    public DateOnly DateEnd { get; set; }
    public ulong Rate1 { get; set; }
    public ulong Rate2 { get; set; }
    public ulong Rate3 { get; set; }
    public ulong Rate4 { get; set; }
    public ulong Rate5 { get; set; }
}