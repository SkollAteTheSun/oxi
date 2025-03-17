using Kp.Ms.Oxi.Models.Entity;
using Kp.Ms.Oxi.Models.Opera;
using System.Xml;
using System.Xml.Serialization;

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

    public async Task<bool> UpdateRoomQuotasByRtavMessage(XmlDocument xml)
    {
        var message =  _xmlDeserializer.DeserializeXmlToMessage<RtavMessage>(xml);
        if (message == null)
        {
            _logService.Log(LogLevel.Error, "Error processing message.");
            return false;
        }

        var roomQuotas = new List<BookingRoomQuota>();

        if (message.HotelReference == null || message.DailyInventories == null)
        {
            _logService.Log(LogLevel.Warning, "Invalid message: No hotel or quota data.");
            return true;
        }

        var roomTypeDict = await GetBookingRoomTypes(message.HotelReference.HotelCode);

        foreach (var inventory in message.DailyInventories)
        {
            if (inventory.RoomTypeInventories == null) continue;

            foreach (var roomTypeInventory in inventory.RoomTypeInventories)
            {
                if (!roomTypeDict.TryGetValue(roomTypeInventory.RoomType, out var roomType))
                {
                    _logService.Log(LogLevel.Warning, $"Room type: {roomTypeInventory.RoomType} not found in database for hotelID: {roomType.HotelId}.");
                    continue;
                }

                var roomQuota = new BookingRoomQuota
                {
                    HotelId = roomType.HotelId,
                    RoomTypeId = roomType.Id,
                    Date = inventory.Datum,
                    AmountAsu = roomTypeInventory.Available
                };

                roomQuotas.Add(roomQuota);
            }
        }

        _logService.Log(LogLevel.Information, $"Successfully matched {roomQuotas.Count} quotas for hotel code: {message.HotelReference.HotelCode}."); ;

        if (roomQuotas != null && roomQuotas.Any())
        {
            await _bookingService.UpdateRoomQuotas(roomQuotas);
        }

        return true;
    }

    public async Task UpdateRoomQuotasByReservation(XmlDocument xml)
    {
        if (xml.DocumentElement.GetAttribute("mfReservationAction") != ReservationAction.ADD.ToString() &&
            xml.DocumentElement.GetAttribute("mfReservationAction") != ReservationAction.CANCEL.ToString())
        {
            return;
        }

        _logService.Log(LogLevel.Information, $"Try update quotas by reservation.");

        var reservation = _xmlDeserializer.DeserializeXmlToMessage<Reservation>(xml);

        if (reservation?.HotelReference?.HotelCode == null || reservation?.RoomStays == null)
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
                _logService.Log(LogLevel.Warning, $"Room type: {roomStay.roomInventoryCode} not found in database for hotelID: {roomType.HotelId}.");
                continue;
            }

            var quotas = await _bookingService.GetRoomQuotas(
                roomType.HotelId,
                roomType.Id,
                roomStay.TimeSpan.StartTime.Date,
                roomStay.TimeSpan.StartTime.Date.AddDays(roomStay.TimeSpan.NumberOfTimeUnits));

            if (!quotas.Any())
            {
                var date = roomStay.TimeSpan.StartTime.Date;
                var toDate = roomStay.TimeSpan.StartTime.Date.AddDays(roomStay.TimeSpan.NumberOfTimeUnits);
                while (date < toDate)
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
                    date = date.AddDays(1);
                }
            }

            if (reservation.MfReservationAction == ReservationAction.ADD.ToString())
            {
                quotas.ForEach(quota => quota.ReservedAsu += 1);
            }

            if (reservation.MfReservationAction == ReservationAction.CANCEL.ToString())
            {
                quotas.ForEach(quota => quota.ReservedAsu -= 1);
            }

            if (quotas != null && quotas.Any())
            {
                await _bookingService.UpdateAmountRoomQuotas(quotas);
            }

            roomQuotas.AddRange(quotas);
        }

        _logService.Log(LogLevel.Information, $"{roomQuotas.Count} quotas by reservation \"{reservation.ReservationId}\" successfully updated.");
    }
    
    public async Task<Dictionary<string, BookingRoomTypes>> GetBookingRoomTypes(string hotelCode)
    {
        var hotelId = await _bookingService.GetHotelId(hotelCode);
        if (hotelId == null)
        {
            _logService.Log(LogLevel.Warning, $"Hotel with code: {hotelCode} not found in database.");
            return new Dictionary<string, BookingRoomTypes>();
        }

        var roomTypes = await _bookingService.GetRoomTypes(hotelId.Value);

        return roomTypes?.ToDictionary(key => key.HrsCode, value => value);
    }
}