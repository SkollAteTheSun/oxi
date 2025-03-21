using Kp.Ms.Oxi.Models.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Kp.Ms.Oxi.EntityConfiguration;

public class TransactionBuilder : IEntityTypeConfiguration<Transaction>
{
    public void Configure(EntityTypeBuilder<Transaction> builder)
    {
        builder.ToTable("transaction");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id).HasColumnName("id");
        builder.Property(x => x.HotelCode).HasColumnName("hotel_code").IsRequired();
        builder.Property(x => x.TransactionId).HasColumnName("transaction_id").IsRequired();
        builder.Property(x => x.InterfaceName).HasColumnName("interface_name").IsRequired();
        builder.Property(x => x.InterfaceType).HasColumnName("interface_type");
        builder.Property(x => x.Sended).HasColumnName("sended").HasDefaultValue(false).IsRequired();
        builder.Property(x => x.Delivered).HasColumnName("delivered").HasDefaultValue(false).IsRequired();
        builder.Property(x => x.DeliveredAt).HasColumnName("delivered_at");
        builder.Property(x => x.SendedAt).HasColumnName("sended_at");
        builder.Property(x => x.Received).HasColumnName("received").HasDefaultValue(false).IsRequired();
        builder.Property(x => x.ReceivedAt).HasColumnName("received_at");
        builder.Property(x => x.Status).HasColumnName("status");
        builder.Property(x => x.Url).HasColumnName("url").IsRequired();
        builder.Property(x => x.CreatedAt).HasColumnName("created_at");
        builder.Property(x => x.UpdatedAt).HasColumnName("updated_at");
        builder.Property(x => x.Request).HasColumnName("request").IsRequired();
        builder.Property(x => x.Xml).HasColumnName("xml").IsRequired();
        builder.Property(x => x.Pre).HasColumnName("pre").HasDefaultValue(false).IsRequired();

        builder.HasIndex(b => new { b.HotelCode, b.InterfaceName }, "ix_hotel_code_and_interface_name");
    }
}