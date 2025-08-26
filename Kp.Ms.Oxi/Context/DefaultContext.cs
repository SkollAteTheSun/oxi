using Kp.Ms.Oxi.EntityConfiguration;
using Kp.Ms.Oxi.Models.Entity;
using Microsoft.EntityFrameworkCore;

namespace Kp.Ms.Oxi.Context;

public class DefaultContext : DbContext
{
    public DefaultContext(DbContextOptions<DefaultContext> options)
        : base(options)
    {
    }

    public DbSet<Transaction> Transactions { get; set; }
    public DbSet<CheckOut> CheckOuts { get; set; }
    public DbSet<CheckOutStatus> CheckOutStatuses { get; set; }
    public DbSet<Hotel> Hotels { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new TransactionBuilder());
        modelBuilder.ApplyConfiguration(new CheckOutBuilder());
        modelBuilder.ApplyConfiguration(new CheckOutStatusBuilder());
        modelBuilder.ApplyConfiguration(new HotelBuilder());
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder
            .UseSnakeCaseNamingConvention();
}