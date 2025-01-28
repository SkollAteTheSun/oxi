using Kp.Ms.Oxi.Models.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Kp.Ms.Oxi.EntityConfiguration;

public class HotelBuilder : IEntityTypeConfiguration<Hotel>
{
    public void Configure(EntityTypeBuilder<Hotel> builder)
    {
        builder.ToTable("hotel");

        builder.HasKey(hotel => hotel.Id);

        builder
            .Property(hotel => hotel.Id)
            .HasColumnName("id")
            .HasDefaultValueSql("gen_random_uuid()");
        builder
            .Property(hotel => hotel.Name)
            .HasColumnName("name")
            .IsRequired();
        builder
            .Property(hotel => hotel.Code)
            .HasColumnName("code")
            .IsRequired();
        builder
            .Property(hotel => hotel.CreatedAt)
            .HasColumnName("created_at")
            .HasDefaultValueSql("CURRENT_TIMESTAMP");

        builder.HasIndex(hotel => hotel.Code, "ix_hotel_code");
    }
}