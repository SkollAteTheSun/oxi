using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Kp.Ms.Oxi.Models.Entity;

public class BookingRoomQuota
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public ulong Id { get; set; }

    [ForeignKey("Hotel")]
    public ulong HotelId { get; set; }
    public BookingHotel Hotel { get; set; }

    [ForeignKey("RoomType")]
    public ulong RoomTypeId { get; set; }
    public BookingRoomTypes RoomType { get; set; }

    [Required]
    public DateTime Date { get; set; }

    [Required]
    public long Amount { get; set; }

    public ulong? RateId { get; set; }
    public bool? Close { get; set; } = false;
}