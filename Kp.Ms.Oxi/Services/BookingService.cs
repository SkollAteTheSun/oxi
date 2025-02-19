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
            .Where(rt => rt.HotelId == hotelId && rt.HrsCode != string.Empty)
            .Select(rt => new
            {
                HrsCode = rt.HrsCode,
                rt.Id
            })
            .ToListAsync();

        return roomTypes.ToDictionary(rt => rt.HrsCode, rt => (ulong?)rt.Id);
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
}
