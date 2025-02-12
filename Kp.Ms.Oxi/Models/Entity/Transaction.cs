namespace Kp.Ms.Oxi.Models.Entity;

public class Transaction
{
    public long Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public bool Sended { get; set; }
    public DateTime? SendedAt { get; set; }
    public bool Received { get; set; }
    public DateTime? ReceivedAt { get; set; }
    public required string HotelCode { get; set; }
    public required string TransactionId { get; set; }
    public required string InterfaceName { get; set; }
    public required int InterfaceType { get; set; }
    public required string Status { get; set; }
    public required string Request { get; set; }
    public required string Url { get; set; }
    public required string Xml { get; set; }
    public bool Pre { get; set; }
}