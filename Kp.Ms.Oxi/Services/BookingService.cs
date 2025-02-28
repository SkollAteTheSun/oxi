using Kp.Ms.Oxi.Context;
using Kp.Ms.Oxi.Models.Entity;
using Microsoft.EntityFrameworkCore;

namespace Kp.Ms.Oxi.Services;

public class BookingService
{
    private readonly IDbContextFactory<BookingContext> _contextFactory;
    private readonly LogService _logService;

    public BookingService(IDbContextFactory<BookingContext> contextFactory, LogService logService)
    {
        _contextFactory = contextFactory;
        _logService = logService;
    }

    public async Task<List<BookingRoomType>> GetRoomTypes(ulong hotelId)
    {
        using var context = _contextFactory.CreateDbContext();
        var roomTypes = await context.RoomTypes
            .AsNoTracking()
            .Where(rt => rt.HotelId == hotelId)
            .Where(rt => !string.IsNullOrEmpty(rt.HrsCode))
            .ToListAsync();

        return roomTypes;
    }

    public async Task<BookingRoomType> GetRoomType(ulong hotelId, string hrsCode)
    {
        using var context = _contextFactory.CreateDbContext();
        var roomTypes = await context.RoomTypes
            .AsNoTracking()
            .FirstOrDefaultAsync(rt => rt.HotelId == hotelId && rt.HrsCode == hrsCode);

        return roomTypes;
    }

    public async Task<BookingHotel> GetHotel(string hotelCode)
    {
        using var context = _contextFactory.CreateDbContext();
        var hotel = await context.Hotels
            .AsNoTracking()
            .FirstOrDefaultAsync(h => h.HrsCode == hotelCode);

        if (hotel == null)
        {
            _logService.Log(LogLevel.Warning, $"Hotel with code: {hotelCode} not found in database.");
            throw new ArgumentException(nameof(hotelCode));
        }

        return hotel;
    }

    public async Task<List<BookingRoomQuota>> GetRoomQuotas(ulong hotelId, ulong roomTypeId, DateTime fromDate, DateTime toDate)
    {
        _logService.Log(LogLevel.Information, $"Start getting quotas.");

        try
        {
            using var context = _contextFactory.CreateDbContext();
            var existingQuotas = await context.RoomQuotas
                .AsNoTracking()
                .Where(q => q.HotelId == hotelId)
                .Where(q => q.RoomTypeId == roomTypeId)
                .Where(q => q.Date >= fromDate && q.Date < toDate)
                .ToListAsync();

            _logService.Log(LogLevel.Information, $"{existingQuotas.Count} quotas got.");

            return existingQuotas;
        }
        catch (Exception ex)
        {
            _logService.Log(LogLevel.Error, $"Error getting quotas: {ex.Message}");
            throw;
        }
    }

    public async Task<BookingReservationAsu?> GetReservationAsu(ulong hotelId, string externalId)
    {
        _logService.Log(LogLevel.Information, $"Start gettting reservation asu: {hotelId}-{externalId}.");

        try
        {
            using var context = _contextFactory.CreateDbContext();

            var reservationAsu = await context.ReservationsAsu
                .FirstOrDefaultAsync(reservation => reservation.HotelId == hotelId && reservation.ExternalId == externalId);

            _logService.Log(LogLevel.Information, "Reservation got successfully.");

            return reservationAsu;
        }
        catch (Exception ex)
        {
            _logService.Log(LogLevel.Error, $"Error upserting reservation: {ex.Message}");
            throw;
        }
    }

    public async Task UpsertReservationAsu(BookingReservationAsu reservation)
    {
        _logService.Log(LogLevel.Information, $"Start upsert reservation: {reservation.ExternalId}.");

        try
        {
            using var context = _contextFactory.CreateDbContext();

            context.ReservationsAsu.Update(reservation);

            await context.SaveChangesAsync();
            _logService.Log(LogLevel.Information, "Reservation upsert successfully.");
        }
        catch (Exception ex)
        {
            _logService.Log(LogLevel.Error, $"Error upserting reservation: {ex.Message}");
            throw;
        }
    }

    public async Task UpdateAmountRoomQuotas(List<BookingRoomQuota> quotas)
    {
        _logService.Log(LogLevel.Information, $"Start updating quotas: {quotas.Count} records.");

        try
        {
            using var context = _contextFactory.CreateDbContext();
            
            context.RoomQuotas.UpdateRange(quotas);

            await context.SaveChangesAsync();
            _logService.Log(LogLevel.Information, "Quotas updated successfully.");
        }
        catch (Exception ex)
        {
            _logService.Log(LogLevel.Error, $"Error updating quotas: {ex.Message}");
            throw;
        }
    }

    public async Task UpdateRoomQuotas(List<BookingRoomQuota> quotas)
    {
        _logService.Log(LogLevel.Information, $"Start updating quotas: {quotas.Count} records.");

        try
        {
            using var context = _contextFactory.CreateDbContext();
            var existingQuotas = await context.RoomQuotas
                .Where(q => quotas.Select(x => x.HotelId).Contains(q.HotelId) &&
                            quotas.Select(x => x.RoomTypeId).Contains(q.RoomTypeId) &&
                            quotas.Select(x => x.Date).Contains(q.Date))
                .ToListAsync();

            foreach (var quota in quotas)
            {
                var existingQuota = existingQuotas
                    .FirstOrDefault(q => q.HotelId == quota.HotelId && q.RoomTypeId == quota.RoomTypeId && q.Date == quota.Date);

                if (existingQuota != null)
                {
                    existingQuota.AmountAsu = quota.AmountAsu;
                }
                else
                {
                    context.RoomQuotas.Add(quota);
                }
            }

            await context.SaveChangesAsync();
            _logService.Log(LogLevel.Information, "Quotas updated successfully.");
        }
        catch (Exception ex)
        {
            _logService.Log(LogLevel.Error, $"Error updating quotas: {ex.Message}");
            throw;
        }
    }
}
