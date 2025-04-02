using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Kp.Ms.Oxi.Models.Entity;

public class BookingRoomRates
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id { get; set; }

    [Required]
    public long HotelId { get; set; }

    [Required]
    public long RoomTypeId { get; set; }

    [Required]
    public long RateId { get; set; }

    [Required]
    public DateOnly Date { get; set; }

    [Required]
    public byte Adults { get; set; }

    [Required]
    public byte Children { get; set; }

    [Required]
    public byte Babies { get; set; }

    [Required]
    public bool Active { get; set; } = true;

    [Required]
    public long Amount { get; set; }

    [Required]
    public byte ExtraBed { get; set; } = 0;

    [Required]
    public byte Craddle { get; set; } = 0;

    [Required]
    public byte CloseSale { get; set; } = 0;

    public byte? Used { get; set; } = 0;

    [Required]
    public int BasePrice { get; set; } = 0;

    [Required]
    public int BabyPrice { get; set; } = 0;

    [Required]
    public int CraddlePrice { get; set; } = 0;

    [Required]
    public int ChildrenPrice { get; set; } = 0;

    [Required]
    public int BabyPriceOnBeds { get; set; } = 0;

    [Required]
    public int ChildrenPriceOnBeds { get; set; } = 0;

    [Required]
    public int AdultPriceOnBeds { get; set; } = 0;
}