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
    private readonly int _chunkSize;

    public BookingService(IConfiguration configuration, IDbContextFactory<BookingContext> contextFactory, IDynamicBookingContextFactory contextDynamicFactory, LogService logService)
    {
        _contextFactory = contextFactory;
        _contextDynamicFactory = contextDynamicFactory;
        _logService = logService;
        _chunkSize = configuration.GetValue<int?>("Processing:ChunkSize") ?? throw new ArgumentNullException("Processing");
    }

    public async Task<ulong?> GetHotelId(string hotelCode)
    {
        using var context = _contextFactory.CreateDbContext();
        var hotelId = await context.Hotels
            .AsNoTracking()
            .Where(h => h.HrsCode == hotelCode)
            .Select(h => (ulong?)h.Id)
            .FirstOrDefaultAsync();

        if (hotelId == null)
        {
            _logService.Log(LogLevel.Warning, $"Hotel with code: {hotelCode} not found in database.");
        }

        return hotelId;
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

    public async Task UpdateAmountRoomQuotas(List<BookingRoomQuota> quotas)
    {
        _logService.Log(LogLevel.Information, $"Start updating quotas: {quotas.Count} records.");

        try
        {
            using var context = _contextFactory.CreateDbContext();
            
            context.UpdateRange(quotas);

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

    public async Task<ulong?> GetRateId(string hrsCode, ulong hotelId)
    {
        using var context = _contextFactory.CreateDbContext();
        var rateId = await context.Rates
            .AsNoTracking()
            .Where(r => r.HrsCode == hrsCode && r.HotelId == hotelId)
            .Select(r => (ulong?)r.Id)
            .FirstOrDefaultAsync();

        if (rateId == null)
        {
            _logService.Log(LogLevel.Warning, $"Rate with HrsCode: {hrsCode} for hotel ID: {hotelId} not found");
        }

        return rateId;
    }

    public async Task UpdateRateDetails(List<BookingRoomRates> bookingRoomRates, string tableName)
    {
        await using var context = _contextDynamicFactory.Create(tableName);

        try
        {
            var chunks = bookingRoomRates.Chunk(_chunkSize);

            foreach (var chunk in chunks)
            {
                await context.BulkInsertOrUpdateAsync(chunk, config =>
                    {
                        config.SetSynchronizeFilter<BookingRoomRates>(r =>
                            chunk.Select(c => new
                            {
                                c.HotelId,
                                c.RoomTypeId,
                                c.RateId,
                                c.Date,
                                c.Adults,
                                c.Children,
                                c.Babies,
                                c.ExtraBed,
                                c.Craddle
                            }).Contains(new
                            {
                                r.HotelId,
                                r.RoomTypeId,
                                r.RateId,
                                r.Date,
                                r.Adults,
                                r.Children,
                                r.Babies,
                                r.ExtraBed,
                                r.Craddle
                            })
                        );
                    });
            }

        }
        catch(Exception ex)
        {
            _logService.Log(LogLevel.Error, $"Error updating rate details: {ex.Message}");
            throw;
        }
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

    public async Task<Dictionary<string, ulong>> GetRoomTypesByHrsCodes(ulong hotelId, IEnumerable<string> hrsCodes)
    {
        using var context = _contextFactory.CreateDbContext();

        var roomTypes = await context.RoomTypes
            .AsNoTracking()
            .Where(rt => rt.HotelId == hotelId)
            .Where(rt => hrsCodes.Contains(rt.HrsCode))
            .ToListAsync();

        return roomTypes.ToDictionary(rt => rt.HrsCode, rt => rt.Id);
    }

    public async Task<Dictionary<string, BookingRoomTypes>> GetRoomTypesByHrsCodes2(ulong hotelId, IEnumerable<string> hrsCodes)
    {
        using var context = _contextFactory.CreateDbContext();

        var roomTypes = await context.RoomTypes
            .AsNoTracking()
            .Where(rt => rt.HotelId == hotelId)
            .Where(rt => hrsCodes.Contains(rt.HrsCode))
            .ToListAsync();

        return roomTypes.ToDictionary(rt => rt.HrsCode, rt => rt);
    }
}