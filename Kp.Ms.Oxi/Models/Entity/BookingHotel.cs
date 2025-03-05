using System.ComponentModel.DataAnnotations.Schema;

namespace Kp.Ms.Oxi.Models.Entity;

public class BookingHotel
{
    public ulong Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Url { get; set; }

    public bool? Active { get; set; }

    public string? Data { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public DateTime? DeletedAt { get; set; }

    public string? AlertBox { get; set; }

    [Column("validateUKId")]
    public bool? ValidateUkid { get; set; }

    public string? DestinationName { get; set; }

    public string? Description { get; set; }

    public string? Facilities { get; set; }

    public string? Conveniences { get; set; }

    public string? Images { get; set; }

    public string? HrsCode { get; set; }

    public string? ShortDescription { get; set; }
}