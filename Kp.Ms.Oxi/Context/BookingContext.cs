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

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<RoomRatesVector>()
            .HasNoKey();
        
        modelBuilder.Entity<RoomRatesVectorAgg>()
            .HasNoKey();
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder
            .UseSnakeCaseNamingConvention();
}