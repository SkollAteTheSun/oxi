using Kp.Ms.Oxi.Context;
using Kp.Ms.Oxi.Models.Entity;
using Microsoft.EntityFrameworkCore;
using EFCore.BulkExtensions;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Kp.Ms.Oxi.Interfaces;

namespace Kp.Ms.Oxi.Services;

public class BookingService
{
    private readonly IConfiguration _configuration;
    private readonly IDbContextFactory<BookingContext> _contextFactory;
    private readonly IDynamicBookingContextFactory _contextDynamicFactory;
    private readonly LogService _logService;

    public BookingService(IConfiguration configuration, IDbContextFactory<BookingContext> contextFactory, IDynamicBookingContextFactory contextDynamicFactory, LogService logService)
    {
        _contextFactory = contextFactory;
        _contextDynamicFactory = contextDynamicFactory;
        _logService = logService;
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

    public async Task<int> GetReservedKpCount(BookingRoomTypes roomType, DateTime date)
    {
        using var context = _contextFactory.CreateDbContext();
        var reserved = await context.Reservations
            .Where(reservation => reservation.HotelId == roomType.HotelId)
            .Where(reservation => reservation.RoomTypeId == roomType.Id)
            .Where(reservation => reservation.DateIn <= date.Date)
            .Where(reservation => reservation.DateOut > date.Date)
            .Where(reservation => reservation.Status != BookingReservationStatuses.cancelled.ToString())
            .Where(reservation => reservation.Status != BookingReservationStatuses.refund.ToString())
            .Where(reservation => reservation.Status != BookingReservationStatuses.closed.ToString())
            .CountAsync();

        return reserved;
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

    public async Task<List<BookingRoomQuota>> GetRoomQuotas(List<BookingRoomQuota> quotas)
    {
        _logService.Log(LogLevel.Information, $"Start getting quotas.");

        try
        {
            using var context = _contextFactory.CreateDbContext();
            var existingQuotas = await context.RoomQuotas
                .AsNoTracking()
                .Where(q => quotas.Select(x => x.HotelId).Contains(q.HotelId) &&
                            quotas.Select(x => x.RoomTypeId).Contains(q.RoomTypeId) &&
                            quotas.Select(x => x.Date).Contains(q.Date))
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

    public async Task UpdateRoomQuotas(List<BookingRoomQuota> quotas)
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

    public async Task UpdateAmountAsuRoomQuotas(List<BookingRoomQuota> quotas)
    {
        _logService.Log(LogLevel.Information, $"Start updating quotas: {quotas.Count} records.");

        try
        {
            var existingQuotas = await GetRoomQuotas(quotas);

            foreach (var quota in quotas)
            {
                var existingQuota = existingQuotas
                    .FirstOrDefault(q => q.HotelId == quota.HotelId && q.RoomTypeId == quota.RoomTypeId && q.Date == quota.Date);

                if (existingQuota != null)
                {
                    existingQuota.Amount = quota.Amount != null ? quota.Amount : existingQuota.Amount;
                    existingQuota.AmountAsu = quota.AmountAsu;
                    existingQuota.ReservedAsu = quota.ReservedAsu;
                }
                else
                {
                    quota.Amount = quota.Amount != null ? quota.Amount : 0;
                    existingQuotas.Add(quota);
                }
            }

            await UpdateRoomQuotas(existingQuotas);

            _logService.Log(LogLevel.Information, "Quotas updated successfully.");
        }
        catch (Exception ex)
        {
            _logService.Log(LogLevel.Error, $"Error updating quotas: {ex.Message}");
            throw;
        }
    }

    public async Task UpdateRateDetails(List<BookingRoomRates> bookingRoomRates, string tableName)
    {
        await using var context = _contextDynamicFactory.Create(tableName);

        try
        {
            if (!bookingRoomRates.Any()) return;

            var values = string.Join(",", bookingRoomRates.Select(c =>
                $"({c.HotelId}, {c.RoomTypeId}, {c.RateId}, '{c.Date:yyyy-MM-dd}', {c.Adults}, {c.Children}, {c.Babies}, {c.ExtraBed}, {c.Craddle})"));

            var deleteSql = $@"
            DELETE FROM {tableName} 
            WHERE (hotel_id, room_type_id, rate_id, date, adults, children, babies, extra_bed, craddle) IN ({values})";

            await context.Database.ExecuteSqlRawAsync(deleteSql);

            await context.BulkInsertAsync(bookingRoomRates);
        }
        catch (Exception ex)
        {
            _logService.Log(LogLevel.Error, $"Error updating rate details: {ex.Message}");
            throw;
        }
    }

    public async Task<BookingRoomTypes> GetRoomType(ulong hotelId, string hrsCode)
    {
        using var context = _contextFactory.CreateDbContext();
        var roomTypes = await context.RoomTypes
            .AsNoTracking()
            .FirstOrDefaultAsync(rt => rt.HotelId == hotelId && rt.HrsCode == hrsCode);

        return roomTypes;
    }

    public async Task<List<BookingRoomTypes>> GetRoomTypes(ulong hotelId)
    {
        using var context = _contextFactory.CreateDbContext();
        var roomTypes = await context.RoomTypes
            .AsNoTracking()
            .Where(rt => rt.HotelId == hotelId)
            .Where(rt => !string.IsNullOrEmpty(rt.HrsCode))
            .ToListAsync();

        return roomTypes;
    }

    public async Task<(ulong? HotelId, Dictionary<string, BookingRoomTypes> RoomTypes, ulong? RateId)>
        GetHotelDataAsync(string hotelCode, IEnumerable<string> roomTypeCodes, string rateCode)
    {
        using var context = _contextFactory.CreateDbContext();

        var hotelId = await context.Hotels
            .AsNoTracking()
            .Where(h => h.HrsCode == hotelCode)
            .Select(h => (ulong?)h.Id)
            .FirstOrDefaultAsync();

        var roomTypes = await context.RoomTypes
            .AsNoTracking()
            .Where(rt => rt.HotelId == hotelId)
            .Where(rt => roomTypeCodes.Contains(rt.HrsCode))
            .ToListAsync();

        var rateId = await context.Rates
            .AsNoTracking()
            .Where(r => r.HrsCode == rateCode && r.HotelId == hotelId && r.DeletedAt == null)
            .Select(r => (ulong?)r.Id)
            .FirstOrDefaultAsync();

        return (hotelId, roomTypes.ToDictionary(rt => rt.HrsCode, rt => rt), rateId);
    }
}