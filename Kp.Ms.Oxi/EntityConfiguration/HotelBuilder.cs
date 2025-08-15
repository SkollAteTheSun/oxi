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
                Id = new Guid("8693c019-76e4-4380-9b24-ee2c71e97290"),
                Name="Риксос Красная Поляна Сочи",
                Code="RIXOS"
            },
            new Hotel
            {
                Id = new Guid("23e771c2-5515-41cc-87fc-962fa0496b65"),
                Name="Сочи Марриотт Красная Поляна",
                Code="AERMC"
            },
            new Hotel
            {
                Id = new Guid("b89aef32-3697-472d-b668-72275605ef24"),
                Name="Новотель Фит Красная Поляна",
                Code="HB769"
            },
            new Hotel
            {
                Id = new Guid("b11a13f3-78d9-47a6-9174-090735f87dad"),
                Name="Новотель Резорт и спа Красная Поляна",
                Code="HB530"
            },
            new Hotel
            {
                Id = new Guid("9bc381f2-999d-4812-bbe5-753e1494dd6e"),
                Name="Мовенпик Красная Поляна",
                Code="HB755"
            },
            new Hotel
            {
                Id = new Guid("4de0f494-9c82-4702-8f1c-dbfa6543bb47"),
                Name="Долина 960",
                Code="SCODL"
            },
            new Hotel
            {
                Id = new Guid("f43fbefb-2176-41c1-90f3-8971adf746ee"),
                Name="Ибис Стайлс Красная Поляна",
                Code="HB754"
            },
            new Hotel
            {
                Id = new Guid("b9de80f5-2710-4e4e-8c9b-c7ecd8279dcf"),
                Name="Панорама by Mercure Красная Поляна",
                Code="HB933"
            },
            new Hotel
            {
                Id = new Guid("073f7916-66ed-406e-82e6-48a09db25190"),
                Name="Кортъярд Марриотт Сочи Красная Поляна",
                Code="AERCS"
            },
            new Hotel
            {
                Id = new Guid("7cdfb7d5-9bd0-4e74-a2a4-f43844f53179"),
                Name="Апартаменты Курорта Красная Поляна",
                Code="APART"
            },
            new Hotel
            {
                Id = new Guid("526fdd1e-595d-4379-b04d-797c92e0b2a0"),
                Name="Отель-замок Богатырь",
                Code="BOGATYR"
            }
        ]);
    }
}