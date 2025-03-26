using Kp.Ms.Oxi.Models.Entity;
using Microsoft.EntityFrameworkCore;

namespace Kp.Ms.Oxi.Context;

public class DynamicBookingContext : DbContext
{
    public string TableName { get; private set; } = string.Empty;

    public DynamicBookingContext(DbContextOptions<DynamicBookingContext> options)
        : base(options)
    {
    }

    public DbSet<BookingRoomRates> BookingRoomRates { get; set; }

    public void SetTableName(string tableName)
    {
        TableName = tableName;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        if (!string.IsNullOrEmpty(TableName))
        {
            modelBuilder.Entity<BookingRoomRates>().ToTable(TableName, t => t.ExcludeFromMigrations());
        }
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (string.IsNullOrEmpty(TableName))
        {
            throw new InvalidOperationException("TableName must be set before using the context.");
        }

        optionsBuilder.UseSnakeCaseNamingConvention();
    }
}