using Kp.Ms.Oxi.Context;
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

    public async Task<Dictionary<string, ulong?>> GetRoomTypeDictionary(ulong hotelId)
    {
        using var context = _contextFactory.CreateDbContext();
        var roomTypes = await context.RoomTypes
            .AsNoTracking()
            .Where(rt => rt.HotelId == hotelId)
            .Select(rt => new { rt.HrsCode, rt.Id })
            .ToListAsync();

        // Используем ulong? в случае, если значение может быть null
        return roomTypes.ToDictionary(rt => rt.HrsCode, rt => (ulong?)rt.Id);
    }

    public async Task<Dictionary<string, ulong?>> GetRoomTypeDictionary2(ulong hotelId)
    {
        using var context = _contextFactory.CreateDbContext();
        var roomTypes = await context.RoomTypes
            .AsNoTracking()
            .Where(rt => rt.HotelId == hotelId && rt.HrsCode != string.Empty)
            .Select(rt => new
            {
                HrsCode = rt.HrsCode,
                rt.Id
            })
            .ToListAsync();

        return roomTypes.ToDictionary(rt => rt.HrsCode, rt => (ulong?)rt.Id);
    }

    public async Task<ulong?> GetRoomTypeIdByCode(string roomTypeCode, ulong hotelId)
    {
        using var context = _contextFactory.CreateDbContext();
        var roomTypeId = await context.RoomTypes
            .AsNoTracking()
            .Where(rt => rt.HrsCode == roomTypeCode && rt.HotelId == hotelId)
            .Select(rt => (ulong?)rt.Id)
            .FirstOrDefaultAsync();

        if (roomTypeId == null)
        {
            _logService.Log(LogLevel.Warning, $"Room type: {roomTypeCode} not found in database for hotelID: {hotelId}.");
        }

        return roomTypeId;
    }

    /*
    public async Task<Dictionary<string, ulong>> GetRoomTypeDictionary(ulong hotelId)
    {
        using var context = await _contextFactory.CreateDbContextAsync();
        return await context.RoomTypes
            .Where(rt => rt.HotelId == hotelId)
            .ToDictionaryAsync(rt => rt.Code, rt => rt.Id );
    }
    */

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
}
