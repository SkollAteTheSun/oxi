namespace Kp.Ms.Oxi.Models.Entity;

public class CheckOut
{
    public Guid Id { get; set; }
    public string PmsId { get; set; }
    public string ExternalId { get; set; }
    public DateTime CheckOutDate { get; set; }
    public string HotelCode { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public CheckOut()
    { 
        Id = Guid.NewGuid();
        UpdatedAt = DateTime.UtcNow;
    }
}
