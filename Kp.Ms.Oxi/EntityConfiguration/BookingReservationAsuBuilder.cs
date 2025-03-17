using Kp.Ms.Oxi.Models.Entity;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Kp.Ms.Oxi.EntityConfiguration;

public class BookingReservationAsuBuilder : IEntityTypeConfiguration<BookingReservationAsu>
{
    public void Configure(EntityTypeBuilder<BookingReservationAsu> builder)
    {
        builder.HasKey(e => e.Id).HasName("PRIMARY");

        builder
            .ToTable("reservations_asu")
            .HasCharSet("utf8mb4")
            .UseCollation("utf8mb4_unicode_ci");

        builder.HasIndex(e => new { e.RoomTypeId, e.DateIn, e.DateOut, e.Status }, "reservation_count_index");

        builder.HasIndex(e => new { e.RoomTypeId, e.DateIn, e.DateOut, e.Status, e.RateId }, "reservation_count_index_new");

        builder.HasIndex(e => e.DateIn, "reservations_asu_date_in_index");

        builder.HasIndex(e => e.DateOut, "reservations_asu_date_out_index");

        builder.HasIndex(e => e.GuestId, "reservations_asu_guest_id_foreign");

        builder.HasIndex(e => e.HotelId, "reservations_asu_hotel_id_foreign");

        builder.HasIndex(e => new { e.HotelId, e.ExternalId }, "reservations_asu_hotel_id_external_id_index")
            .IsUnique();

        builder.HasIndex(e => e.Number, "reservations_asu_number_index");

        builder.HasIndex(e => e.OrderSource, "reservations_asu_order_source_index");

        builder.HasIndex(e => e.PaymentMethodId, "reservations_asu_payment_method_id_foreign");

        builder.HasIndex(e => e.RateId, "reservations_asu_rate_id_foreign");

        builder.HasIndex(e => e.RoomTypeId, "reservations_asu_room_type_id_foreign");

        builder.HasIndex(e => e.Status, "reservations_asu_status_index");

        builder.Property(e => e.Id)
            .HasColumnType("bigint(20) unsigned")
            .HasColumnName("id");
        builder.Property(e => e.Adults)
            .HasDefaultValueSql("'0'")
            .HasColumnType("tinyint(4)")
            .HasColumnName("adults");
        builder.Property(e => e.Babies)
            .HasDefaultValueSql("'0'")
            .HasColumnType("tinyint(4)")
            .HasColumnName("babies");
        builder.Property(e => e.ChangedBy)
            .HasColumnType("int(11)")
            .HasColumnName("changed_by");
        builder.Property(e => e.CheckOutDate)
            .HasColumnType("datetime")
            .HasColumnName("check_out_date");
        builder.Property(e => e.Children)
            .HasDefaultValueSql("'0'")
            .HasColumnType("tinyint(4)")
            .HasColumnName("children");
        builder.Property(e => e.CombinedRates)
            .HasMaxLength(255)
            .HasColumnName("combined_rates");
        builder.Property(e => e.CommentManager)
            .HasColumnType("text")
            .HasColumnName("comment_manager");
        builder.Property(e => e.Craddle)
            .HasDefaultValueSql("'0'")
            .HasColumnType("tinyint(4)")
            .HasColumnName("craddle");
        builder.Property(e => e.CreatedAt)
            .HasColumnType("timestamp")
            .HasColumnName("created_at");
        builder.Property(e => e.Data)
            .HasColumnType("json")
            .HasColumnName("data");
        builder.Property(e => e.DateCancelled)
            .HasColumnType("datetime")
            .HasColumnName("date_cancelled");
        builder.Property(e => e.DateGuaranteed)
            .HasColumnType("datetime")
            .HasColumnName("date_guaranteed");
        builder.Property(e => e.DateIn).HasColumnName("date_in");
        builder.Property(e => e.DateOut).HasColumnName("date_out");
        builder.Property(e => e.DeletedAt)
            .HasColumnType("timestamp")
            .HasColumnName("deleted_at");
        builder.Property(e => e.DontSendPms)
            .HasDefaultValueSql("'0'")
            .HasColumnName("dont_send_pms");
        builder.Property(e => e.ExternalId)
            .HasMaxLength(255)
            .HasDefaultValueSql("''")
            .HasColumnName("external_id");
        builder.Property(e => e.ExtraBed)
            .HasDefaultValueSql("'0'")
            .HasColumnType("tinyint(4)")
            .HasColumnName("extra_bed");
        builder.Property(e => e.Guaranteed).HasColumnName("guaranteed");
        builder.Property(e => e.GuestId)
            .HasColumnType("bigint(20) unsigned")
            .HasColumnName("guest_id");
        builder.Property(e => e.HasRateOwnQuotas).HasColumnName("has_rate_own_quotas");
        builder.Property(e => e.HotelId)
            .HasColumnType("bigint(20) unsigned")
            .HasColumnName("hotel_id");
        builder.Property(e => e.Infant)
            .HasDefaultValueSql("'0'")
            .HasColumnType("int(11)")
            .HasColumnName("infant");
        builder.Property(e => e.IsChangeProcessed).HasColumnName("is_change_processed");
        builder.Property(e => e.IsChanged).HasColumnName("is_changed");
        builder.Property(e => e.IsCheckOut).HasColumnName("is_check_out");
        builder.Property(e => e.MindboxPointsDate).HasColumnName("mindbox_points_date");
        builder.Property(e => e.NeedLink)
            .HasDefaultValueSql("'0'")
            .HasColumnName("need_link");
        builder.Property(e => e.Number).HasColumnName("number");
        builder.Property(e => e.OrderNumber)
            .HasMaxLength(15)
            .HasColumnName("order_number");
        builder.Property(e => e.OrderSource)
            .HasDefaultValueSql("'other'")
            .HasColumnType("enum('oob','mobile','other','site_mantera','oob_mantera','mobile_mantera')")
            .HasColumnName("order_source");
        builder.Property(e => e.PaymentMethodId)
            .HasDefaultValueSql("'1'")
            .HasColumnType("bigint(20) unsigned")
            .HasColumnName("payment_method_id");
        builder.Property(e => e.PointsUsed)
            .HasDefaultValueSql("'0'")
            .HasColumnType("bigint(20) unsigned")
            .HasColumnName("points_used");
        builder.Property(e => e.PriceChanging)
            .HasDefaultValueSql("'0'")
            .HasColumnType("int(11)")
            .HasColumnName("price_changing");
        builder.Property(e => e.Promo)
            .HasMaxLength(255)
            .HasColumnName("promo");
        builder.Property(e => e.PromoId)
            .HasColumnType("bigint(20) unsigned")
            .HasColumnName("promo_id");
        builder.Property(e => e.RateId)
            .HasColumnType("bigint(20) unsigned")
            .HasColumnName("rate_id");
        builder.Property(e => e.RoomQuantity)
            .HasColumnType("tinyint(3) unsigned")
            .HasColumnName("room_quantity");
        builder.Property(e => e.RoomRates)
            .HasColumnType("text")
            .HasColumnName("room_rates");
        builder.Property(e => e.RoomTypeId)
            .HasColumnType("bigint(20) unsigned")
            .HasColumnName("room_type_id");
        builder.Property(e => e.SetPoints)
            .HasColumnType("int(11)")
            .HasColumnName("set_points");
        builder.Property(e => e.Status)
            .HasDefaultValueSql("'new'")
            .HasColumnType("enum('new','approved','paid','cancelled','closed','refund')")
            .HasColumnName("status");
        builder.Property(e => e.StatusLoyalty)
            .HasMaxLength(255)
            .HasColumnName("status_loyalty");
        builder.Property(e => e.Total)
            .HasColumnType("bigint(20) unsigned")
            .HasColumnName("total");
        builder.Property(e => e.UpdatedAt)
            .HasColumnType("timestamp")
            .HasColumnName("updated_at");
    }
}
