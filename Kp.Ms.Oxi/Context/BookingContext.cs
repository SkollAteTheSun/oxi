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
    public DbSet<RoomRatesVector> RoomRatesVectorRows { get; set; }
    public DbSet<RoomRatesVectorAgg> RoomRatesVectorAggRows { get; set; }
    public DbSet<BookingHotel> Hotels { get; set; }
    public DbSet<BookingRoomTypes> RoomTypes { get; set; }
    public DbSet<BookingRoomQuota> RoomQuotas { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<RoomRatesVector>()
            .HasNoKey();
        
        modelBuilder.Entity<RoomRatesVectorAgg>()
            .HasNoKey();

        modelBuilder.Entity<BookingHotel>().ToTable("hotels");
        modelBuilder.Entity<BookingRoomTypes>().ToTable("room_types");
        modelBuilder.Entity<BookingRoomQuota>().ToTable("room_quotas");

        modelBuilder
            .Entity<BookingRoomQuota>()
            .Property(roomQuota => roomQuota.AmountAsu)
            .HasColumnName("amount_asu")
            .HasDefaultValue(0);
        modelBuilder
            .Entity<BookingRoomQuota>()
            .Property(roomQuota => roomQuota.ReservedAsu)
            .HasColumnName("reserved_asu")
            .HasDefaultValue(0); 
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder
            .UseSnakeCaseNamingConvention();
}