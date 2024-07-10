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

        builder.Property(x => x.Id).HasColumnName("id").HasColumnOrder(1);
        builder.Property(x => x.HotelCode).HasColumnName("hotel_code").HasColumnOrder(2).IsRequired();
        builder.Property(x => x.TransactionId).HasColumnName("transaction_id").HasColumnOrder(3).IsRequired();
        builder.Property(x => x.InterfaceName).HasColumnName("interface_name").HasColumnOrder(4).IsRequired();
        builder.Property(x => x.Sended).HasColumnName("sended").HasDefaultValue(false).HasColumnOrder(5);
        builder.Property(x => x.Status).HasColumnName("status").HasColumnOrder(6);
        builder.Property(x => x.Url).HasColumnName("url").HasColumnOrder(7).IsRequired();
        builder.Property(x => x.CreatedAt).HasColumnName("created_at").HasColumnOrder(8);
        builder.Property(x => x.UpdatedAt).HasColumnName("updated_at").HasColumnOrder(9);
        builder.Property(x => x.Request).HasColumnName("request").HasColumnOrder(10).IsRequired();
        builder.Property(x => x.Xml).HasColumnName("xml").HasColumnOrder(11).IsRequired();
    }
}