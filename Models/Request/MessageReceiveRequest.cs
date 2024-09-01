namespace Kp.Ms.Oxi.Models.Request;

public class MessageReceiveRequest
{
    public string? ProprtyName { get; set; }
    public string? MessageType { get; set; }
    public string? TransactionId { get; set; }
    public string? Status { get; set; }
}