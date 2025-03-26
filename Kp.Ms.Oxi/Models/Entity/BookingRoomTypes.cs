using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Kp.Ms.Oxi.Models.Entity;

public class BookingRoomTypes
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public ulong Id { get; set; }

    [Required]
    [MaxLength(255)]
    public string Name { get; set; }

    [Required]
    [MaxLength(255)]
    public string Code { get; set; }

    public byte Capacity { get; set; }
    public int CountMainSeat { get; set; } = 0;
    public string? Description { get; set; }
    public string? Images { get; set; }

    [ForeignKey("Hotel")]
    public ulong HotelId { get; set; }
    public BookingHotel Hotel { get; set; }

    public bool Active { get; set; } = true;
    public string Data { get; set; }
    public DateTime? DeletedAt { get; set; }
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public byte Order { get; set; } = 99;
    public string? AccommodationTypes { get; set; }
    public int? MinNightsRoomType { get; set; } = 1;
    public uint? ResourceId { get; set; }
    public ushort? SquareFrom { get; set; }
    public ushort? SquareTo { get; set; }

    [Column("extrabed_capacity")]
    public byte? ExtraBedCapacity { get; set; } = 0;
    public bool NotAvailForChildren { get; set; } = false;
    public bool NotAvailForBaby { get; set; } = false;

    [MaxLength(255)]
    public string? HrsCode { get; set; }

    public byte? ChildrenCapacity { get; set; } = 0;
    public byte? CradleCapacity { get; set; } = 0;

    [MaxLength(255)]
    public string? Panorama { get; set; }

    public ICollection<BookingRoomQuota> RoomQuotas { get; set; }
}