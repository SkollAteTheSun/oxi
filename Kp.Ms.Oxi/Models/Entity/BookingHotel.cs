using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Kp.Ms.Oxi.Models.Entity;

public class BookingHotel
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public ulong Id { get; set; }

    [Required]
    [MaxLength(255)]
    public string Name { get; set; }

    [MaxLength(255)]
    public string Url { get; set; }

    public bool Active { get; set; } = true;
    public string Data { get; set; }
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public DateTime? DeletedAt { get; set; }
    public string AlertBox { get; set; }
    public bool? ValidateUKId { get; set; } = false;
    public string DestinationName { get; set; }
    public string Description { get; set; }
    public string Facilities { get; set; }
    public string Conveniences { get; set; }
    public string Images { get; set; }

    [MaxLength(255)]
    public string HrsCode { get; set; }

    [MaxLength(255)]
    public string ShortDescription { get; set; }

    public ICollection<BookingRoomTypes> RoomTypes { get; set; }
    public ICollection<BookingRoomQuota> RoomQuotas { get; set; }
}