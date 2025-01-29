namespace Kp.Ms.Oxi.Models.Entity;

public class Hotel
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Code { get; set; }
    public DateTime CreatedAt { get; set; }

    public Hotel()
    {
        Id = Guid.NewGuid();
    }
}
