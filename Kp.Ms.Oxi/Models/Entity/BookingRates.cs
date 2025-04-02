using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Kp.Ms.Oxi.Models.Entity;

public class BookingRates
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

    public string? Description { get; set; }

    [Required]
    public ulong HotelId { get; set; }

    [Required]
    public DateOnly DateStart { get; set; }

    [Required]
    public DateOnly DateEnd { get; set; }

    [Required]
    public bool Active { get; set; } = true;
    public string? Data { get; set; }
    public DateTime? DeletedAt { get; set; }
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    [Required]
    public ulong PartialPaymentAmount { get; set; } = 0;

    [Required]
    public byte MinStay { get; set; } = 1;

    [Required]
    public byte TillCheckIn { get; set; } = 0;
    public ulong? BaseRateId { get; set; }

    [Required]
    public bool Inherited { get; set; } = false;
    public int? MaxStay { get; set; }
    public int? MultipleDays { get; set; }
    public int? FixChildrenExtraBed { get; set; } = 0;
    public int? FixExtraBed { get; set; } = 0;
    public int? FixCraddle { get; set; } = 0;
    public int? FixCountChildren { get; set; } = 0;
    public int? FixCountBabies { get; set; } = 0;
    public string? ConditionCancel { get; set; }

    [Required]
    public byte Priority { get; set; } = 1;
    public DateOnly? FixChildrenExtraBedDateNew { get; set; }
    public int? FixChildrenExtraBedNew { get; set; } = 0;
    public DateOnly? FixExtraBedDateNew { get; set; }
    public int? FixExtraBedNew { get; set; } = 0;
    public DateOnly? FixCraddleDateNew { get; set; }
    public int? FixCraddleNew { get; set; } = 0;
    public DateOnly? FixCountChildrenDateNew { get; set; }
    public int? FixCountChildrenNew { get; set; } = 0;
    public DateOnly? FixCountBabiesDateNew { get; set; }
    public int? FixCountBabiesNew { get; set; } = 0;

    [Required]
    public bool EditToday { get; set; } = false;

    [Required]
    public bool Hidden { get; set; } = false;

    [Required]
    public bool Loyalty { get; set; } = false;

    [Required]
    [MaxLength(16)]
    public string? TariffCharacteristics { get; set; }

    public string? AddData { get; set; }
    public DateOnly? DateStartBooking { get; set; }
    public DateOnly? DateEndBooking { get; set; }

    [Required]
    public byte Order { get; set; } = 99;

    [Required]
    public bool OwnQuotas { get; set; } = false;
    public ulong? RateAfterId { get; set; }
    public ulong? RateBeforeId { get; set; }
    public bool? NonReturnable { get; set; } = false;
    public bool? TillCheckOut { get; set; } = false;
    public ulong? MarketCodeId { get; set; }

    [MaxLength(255)]
    public string? HrsCode { get; set; }

    public bool? NeedInherit { get; set; } = false;
    public string? Stocks { get; set; }
    public uint? PmsLoyaltyMarketCodeId { get; set; }

    [MaxLength(255)]
    public string? HrsCodeLoyalty { get; set; }

    [MaxLength(255)]
    public string? AdditionalSourceCode { get; set; }

    public bool? DisableExtraBed { get; set; } = false;
    public bool? DisableCradle { get; set; } = false;
    public bool? DisableChildren { get; set; } = false;
    public ulong? CancellationPolicyId { get; set; }
}
