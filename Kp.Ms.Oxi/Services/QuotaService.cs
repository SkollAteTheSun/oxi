using Kp.Ms.Oxi.Context;
using Kp.Ms.Oxi.Models.Entity;
using Kp.Ms.Oxi.Models.Opera;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using System.Xml;
using System.Xml.Serialization;

namespace Kp.Ms.Oxi.Services;

public class QuotaService
{
    private readonly LogService _logService;
    private readonly IDbContextFactory<BookingContext> _contextFactory;
    public QuotaService(LogService logService, IDbContextFactory<BookingContext> contextFactory)
    {
        _logService = logService;
        _contextFactory = contextFactory;
    }

    public async Task<List<BookingRoomQuota>> MapToRoomQuotas(RtavMessage message)
    {
        var roomQuotas = new List<BookingRoomQuota>();

        if (message.HotelReference == null || message.DailyInventories == null)
        {
            _logService.Log(LogLevel.Warning, "Invalid message: No hotel or quota data.");
            return roomQuotas;
        }

        var hotelId = await GetHotelIdByCode(message.HotelReference.HotelCode);
        if (hotelId == null)
        {
            _logService.Log(LogLevel.Warning, $"Hotel with code: {message.HotelReference.HotelCode} not found in database.");
            return roomQuotas;
        }

        foreach (var inventory in message.DailyInventories)
        {
            if (inventory.RoomTypeInventories == null) continue;

            foreach (var roomTypeInventory in inventory.RoomTypeInventories)
            {
                var roomTypeId = await GetRoomTypeIdByCode(roomTypeInventory.RoomType, hotelId.Value);
                if (roomTypeId == null) continue;

                var roomQuota = new BookingRoomQuota
                {
                    HotelId = hotelId.Value,
                    RoomTypeId = roomTypeId.Value,
                    Date = inventory.Datum,
                    Amount = roomTypeInventory.Available,
                    Close = roomTypeInventory.Available <= 0
                };

                roomQuotas.Add(roomQuota);
            }
        }

        _logService.Log(LogLevel.Information, $"Successfully matched {roomQuotas.Count} quotas for hotelID: {hotelId.Value}."); ;
        return roomQuotas;
    }

    public RtavMessage? DeserializeXmlToMessage(XmlDocument xmlDocument)
    {
        try
        {
            var rootNode = xmlDocument.DocumentElement;
            if (rootNode == null) return null;

            XmlSerializer serializer = new XmlSerializer(typeof(RtavMessage));
            using var reader = new StringReader(rootNode.OuterXml);
            using var xmlReader = XmlReader.Create(reader);

            return (RtavMessage?)serializer.Deserialize(xmlReader);
        }
        catch (Exception ex)
        {
            _logService.Log(LogLevel.Error, $"XML deserialization error: {ex.Message}");
            return null;
        }
    }

    public async Task<ulong?> GetHotelIdByCode(string hotelCode)
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

    public async Task UpdateRoomQuotas(List<BookingRoomQuota> quotas)
    {
        _logService.Log(LogLevel.Information, $"Start updating quotas: {quotas.Count} records.");

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
                existingQuota.Amount = quota.Amount;
                existingQuota.Close = quota.Close;
            }
            else
            {
                context.RoomQuotas.Add(quota);
            }
        }

        await context.SaveChangesAsync();
        _logService.Log(LogLevel.Information, "Quotas updated successfully.");
    }
}