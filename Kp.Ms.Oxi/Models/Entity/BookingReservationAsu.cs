namespace Kp.Ms.Oxi.Models.Entity;

public class BookingReservationAsu
{
    public ulong Id { get; set; }

    public string? Number { get; set; } = null!;

    public ulong? HotelId { get; set; }

    public ulong? RoomTypeId { get; set; }

    public ulong? RateId { get; set; }

    public ulong? GuestId { get; set; }

    public int? RoomQuantity { get; set; }

    public ulong? Total { get; set; }

    public DateOnly? DateIn { get; set; }

    public DateOnly? DateOut { get; set; }

    public string? Status { get; set; } = null!;

    public string? Data { get; set; }

    public string? CommentManager { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public DateTime? DeletedAt { get; set; }

    public ulong? PaymentMethodId { get; set; }

    public string? OrderNumber { get; set; }

    public int? Adults { get; set; }

    public int? Children { get; set; }

    public int? Babies { get; set; }

    public int? Infant { get; set; }

    public int? ExtraBed { get; set; }

    public int? Craddle { get; set; }

    public bool IsCheckOut { get; set; }

    public DateTime? CheckOutDate { get; set; }

    public DateOnly? MindboxPointsDate { get; set; }

    public ulong? PromoId { get; set; }

    public bool IsChanged { get; set; }

    public bool IsChangeProcessed { get; set; }

    public int? ChangedBy { get; set; }

    public int? PriceChanging { get; set; }

    public string? Promo { get; set; }

    public string? RoomRates { get; set; }

    public bool HasRateOwnQuotas { get; set; }

    public ulong? PointsUsed { get; set; }

    public int? SetPoints { get; set; }

    public bool Guaranteed { get; set; }

    public bool? NeedLink { get; set; }

    public string? CombinedRates { get; set; }

    public string? OrderSource { get; set; } = null!;

    public DateTime? DateCancelled { get; set; }

    public DateTime? DateGuaranteed { get; set; }

    public string? ExternalId { get; set; }

    public bool? DontSendPms { get; set; }

    public string? StatusLoyalty { get; set; }
}
