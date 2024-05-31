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
        builder.Property(x => x.CreatedAt).HasColumnName("created_at");
        builder.Property(x => x.UpdatedAt).HasColumnName("updated_at");
        builder.Property(x => x.Sended).HasColumnName("sended").HasDefaultValue(false);
        builder.Property(x => x.TransactionId).HasColumnName("transaction_id");
        builder.Property(x => x.InterfaсeName).HasColumnName("interface_name");
        builder.Property(x => x.Status).HasColumnName("status");
        builder.Property(x => x.Request).HasColumnName("request");
        builder.Property(x => x.Url).HasColumnName("url");
        builder.Property(x => x.Xml).HasColumnName("xml");
    }
}