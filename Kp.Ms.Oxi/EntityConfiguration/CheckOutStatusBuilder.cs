using Kp.Ms.Oxi.Models.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Kp.Ms.Oxi.EntityConfiguration;

public class CheckOutStatusBuilder : IEntityTypeConfiguration<CheckOutStatus>
{
    public void Configure(EntityTypeBuilder<CheckOutStatus> builder)
    {
        builder.ToTable("check_out_status");

        builder.HasKey(status => status.Id);

        builder
            .Property(status => status.Id)
            .HasColumnName("id")
            .IsRequired();
        builder
            .Property(status => status.BookingId)
            .HasColumnName("booking_id")
            .IsRequired();
        builder
            .Property(status => status.Status)
            .HasColumnName("status")
            .HasConversion<string>()
            .IsRequired();
        builder
            .Property(status => status.Date)
            .HasColumnName("date")
            .IsRequired();

        builder.HasIndex(checkOut => checkOut.BookingId, "ix_check_out_status_booking_id");

        builder
            .HasIndex(checkOut => new { checkOut.BookingId, checkOut.Status }, "ix_check_out_status_booking_id_status")
            .IsUnique();
    }
}
