using Kp.Ms.Oxi.EntityConfiguration;
using Kp.Ms.Oxi.Models.Entity;
using Microsoft.EntityFrameworkCore;

namespace Kp.Ms.Oxi.Context;

public class BookingContext : DbContext
{
    public BookingContext(DbContextOptions<BookingContext> options)
        : base(options)
    {
    }

    // Sql query collections
    public DbSet<BookingHotel> Hotels { get; set; }
    public DbSet<BookingRoomType> RoomTypes { get; set; }
    public DbSet<BookingRoomQuota> RoomQuotas { get; set; }
    public DbSet<BookingReservationAsu> ReservationsAsu { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .Entity<BookingHotel>()
            .ToTable("hotels", t => t.ExcludeFromMigrations());
        modelBuilder
            .Entity<BookingRoomType>()
            .ToTable("room_types", t => t.ExcludeFromMigrations());
        modelBuilder
            .Entity<BookingRoomQuota>()
            .ToTable("room_quotas", t => t.ExcludeFromMigrations());

        modelBuilder.ApplyConfiguration(new BookingReservationAsuBuilder());
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder
            .UseSnakeCaseNamingConvention();
}