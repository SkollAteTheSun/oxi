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
            .IsRequired();
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

        builder.HasData([
            new Hotel
            {
                Name="Риксос Красная Поляна Сочи",
                Code="RIXOS"
            },
            new Hotel
            {
                Name="Сочи Марриотт Красная Поляна",
                Code="AERMC"
            },
            new Hotel
            {
                Name="Новотель Фит Красная Поляна",
                Code="HB769"
            },
            new Hotel
            {
                Name="Новотель Резорт и спа Красная Поляна",
                Code="HB530"
            },
            new Hotel
            {
                Name="Мовенпик Красная Поляна",
                Code="HB755"
            },
            new Hotel
            {
                Name="Долина 960",
                Code="SCODL"
            },
            new Hotel
            {
                Name="Ибис Стайлс Красная Поляна",
                Code="HB754"
            },
            new Hotel
            {
                Name="Панорама by Mercure Красная Поляна",
                Code="HB933"
            },
            new Hotel
            {
                Name="Кортъярд Марриотт Сочи Красная Поляна",
                Code="AERCS"
            },
            new Hotel
            {
                Name="Апартаменты Курорта Красная Поляна",
                Code="APART"
            },
            new Hotel
            {
                Name="Отель-замок Богатырь",
                Code="BOGATYR"
            }
        ]);
    }
}