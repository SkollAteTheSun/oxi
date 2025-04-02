using Kp.Ms.Oxi.Models.Entity;
using Kp.Ms.Oxi.Models.Opera;
using Microsoft.OpenApi.Extensions;

namespace Kp.Ms.Oxi.Services;

public class QuotaService
{
    private readonly LogService _logService;
    private readonly BookingService _bookingService;
    private readonly XmlDeserializer _xmlDeserializer;

    public QuotaService(LogService logService, BookingService bookingService, XmlDeserializer xmlDeserializer)
    {
        _logService = logService;
        _bookingService = bookingService;
        _xmlDeserializer = xmlDeserializer;
    }

    public async Task<bool> UpdateRoomQuotasByRtavMessage(RtavMessage message)
    {
        if (message == null)
        {
            _logService.Log(LogLevel.Error, "Error processing message.");
            return false;
        }

        var roomQuotas = new List<BookingRoomQuota>();

        if (message.HotelReference == null)
        {
            _logService.Log(LogLevel.Warning, "Invalid message: No hotel or quota data.");
            return true;
        }

        var roomTypeDict = await GetBookingRoomTypes(message.HotelReference.HotelCode);  

        foreach (var inventory in message.DailyInventories)
        {
            foreach (var roomTypeInventory in inventory.RoomTypeInventories)
            {
                if (!roomTypeDict.TryGetValue(roomTypeInventory.RoomType, out var roomType))
                {
                    _logService.Log(LogLevel.Warning, $"Room type: {roomTypeInventory.RoomType} not found in database for hotel code: {message.HotelReference.HotelCode}.");
                    continue;
                }

                long? amount = null;

                if (roomType.QuotasFromAsu)
                {
                    var reservedKp = await _bookingService.GetReservedKpCount(roomType, inventory.Datum);
                    amount = roomTypeInventory.Available + reservedKp;
                }

                var roomQuota = new BookingRoomQuota
                {
                    HotelId = roomType.HotelId,
                    RoomTypeId = roomType.Id,
                    Date = inventory.Datum,
                    Amount = amount,
                    AmountAsu = roomTypeInventory.Available,
                    ReservedAsu = roomTypeInventory?.Definite?.ResvCount ?? 0
                };

                roomQuotas.Add(roomQuota);
            }
        }

        _logService.Log(LogLevel.Information, $"Successfully matched {roomQuotas.Count} quotas for hotel code: {message.HotelReference.HotelCode}."); ;

        if (roomQuotas.Any())
        {
            await _bookingService.UpdateAmountAsuRoomQuotas(roomQuotas);
        }

        return true;
    }

    public async Task UpdateRoomQuotasByReservation(Reservation reservation)
    {
        try
        {
            if (reservation == null)
            {
                _logService.Log(LogLevel.Error, "Error processing update room quotas by reservation.");
                return;
            }

            if (reservation.MfReservationAction != ReservationAction.ADD.ToString() &&
                reservation.MfReservationAction != ReservationAction.CANCEL.ToString())
            {
                return;
            }

            _logService.Log(LogLevel.Information, $"Try update quotas by reservation.");

            if (reservation?.HotelReference?.HotelCode == null)
            {
                _logService.Log(LogLevel.Warning, "Invalid message: No hotel or room data.");
                return;
            }

            var roomQuotas = new List<BookingRoomQuota>();

            var roomTypeDict = await GetBookingRoomTypes(reservation.HotelReference.HotelCode);

            foreach (var roomStay in reservation.RoomStays)
            {
                if (roomStay.roomInventoryCode == null || roomStay.TimeSpan.TimeUnitType != "DAY")
                {
                    continue;
                }

                if (!roomTypeDict.TryGetValue(roomStay.roomInventoryCode, out var roomType))
                {
                    _logService.Log(LogLevel.Warning, $"Room type: {roomStay.roomInventoryCode} not found in database for hotel code: {reservation.HotelReference.HotelCode}.");
                    continue;
                }

                var quotas = await _bookingService.GetRoomQuotas(
                    roomType.HotelId,
                    roomType.Id,
                    roomStay.TimeSpan.StartTime.Date,
                    roomStay.TimeSpan.StartTime.Date.AddDays(roomStay.TimeSpan.NumberOfTimeUnits));

                for (var i = 0; i < roomStay.TimeSpan.NumberOfTimeUnits; i++)
                {
                    var date = roomStay.TimeSpan.StartTime.Date.AddDays(i);

                    if (!quotas.Any(quota => quota.Date.Date == date))
                    {
                        quotas.Add(new BookingRoomQuota()
                        {
                            HotelId = roomType.HotelId,
                            RoomTypeId = roomType.Id,
                            Date = date,
                            Amount = 0,
                            Close = false,
                            AmountAsu = 0,
                            ReservedAsu = 0
                        });
                    }
                }

                quotas.ForEach(quota => quota.ReservedAsu = reservation.MfReservationAction == ReservationAction.ADD.ToString()
                    ? quota.ReservedAsu + 1
                    : quota.ReservedAsu - 1);

                if (quotas.Any())
                {
                    await _bookingService.UpdateRoomQuotas(quotas);
                }

                roomQuotas.AddRange(quotas);
            }

            _logService.Log(LogLevel.Information, $"{roomQuotas.Count} quotas by reservation \"{reservation.ReservationId}\" successfully updated.");
        }
        catch (Exception ex)
        {
            _logService.Log(LogLevel.Error, $"Failed to update room quotas by reservation: {ex.Message}");
        }
    }

    private async Task<Dictionary<string, BookingRoomTypes>> GetBookingRoomTypes(string hotelCode)
    {
        var hotel = await _bookingService.GetHotel(hotelCode);
        if (hotel == null)
        {
            _logService.Log(LogLevel.Warning, $"Hotel with code: {hotelCode} not found in database.");
            return new Dictionary<string, BookingRoomTypes>();
        }

        var roomTypes = await _bookingService.GetRoomTypes(hotel.Id);

        return roomTypes?.ToDictionary(key => key.HrsCode, value => value);
    }

}