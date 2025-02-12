using Kp.Ms.Oxi.Models.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Kp.Ms.Oxi.EntityConfiguration;

public class CheckOutBuilder : IEntityTypeConfiguration<CheckOut>
{
    public void Configure(EntityTypeBuilder<CheckOut> builder)
    {
        builder.ToTable("check_out");

        builder.HasKey(checkOut => checkOut.Id);

        builder
            .Property(checkOut => checkOut.Id)
            .HasColumnName("id")
            .IsRequired();
        builder
            .Property(checkOut => checkOut.PmsId)
            .HasColumnName("pms_id")
            .IsRequired();
        builder
            .Property(checkOut => checkOut.ExternalId)
            .HasColumnName("external_id")
            .IsRequired();
        builder
            .Property(checkOut => checkOut.CheckOutDate)
            .HasColumnName("check_out_date")
            .IsRequired();
        builder
            .Property(checkOut => checkOut.HotelCode)
            .HasColumnName("hotel_code")
            .IsRequired();
        builder
            .Property(checkOut => checkOut.CreatedAt)
            .HasColumnName("created_at")
            .HasDefaultValueSql("CURRENT_TIMESTAMP");
        builder
            .Property(checkOut => checkOut.UpdatedAt)
            .HasColumnName("updated_at");

        builder
            .HasIndex(checkOut => new { checkOut.HotelCode, checkOut.ExternalId }, "ix_check_out_hotel_code_external_id")
            .IsUnique();
    }
}