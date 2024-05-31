namespace Kp.Ms.Oxi.Models.Entity;

public class Transaction
{
    public long Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public bool Sended { get; set; }
    public required string TransactionId { get; set; }
    public required string InterfaсeName { get; set; }
    public required string Status { get; set; }
    public required string Request { get; set; }
    public required string Url { get; set; }
    public required string Xml { get; set; }
}