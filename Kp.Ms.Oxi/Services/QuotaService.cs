using Kp.Ms.Oxi.Exceptions;
using Kp.Ms.Oxi.Models.Entity;
using Kp.Ms.Oxi.Models.Fidelio;
using Kp.Ms.Oxi.Models.Fidelio.Response;
using Kp.Ms.Oxi.Models.Opera;
using Kp.Ms.Oxi.Models.Request.Fidelio.Result;
using Microsoft.OpenApi.Extensions;
using System;
using System.Collections.Generic;

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
                try
                {
                    var roomQuota = await CreateBookingRoomQuota(
                        roomTypeDict,
                        message.HotelReference.HotelCode,
                        roomTypeInventory.RoomType,
                        inventory.Datum,
                        roomTypeInventory.Available,
                        roomTypeInventory.Definite.ResvCount);

                    roomQuotas.Add(roomQuota);
                }
                catch (Exception ex)
                {
                }
            }
        }

        _logService.Log(LogLevel.Information, $"Successfully matched {roomQuotas.Count} quotas for hotel code: {message.HotelReference.HotelCode}."); ;

        if (roomQuotas.Any())
        {
            await _bookingService.UpdateAmountAsuRoomQuotas(roomQuotas);
        }

        return true;
    }

    public async Task<FidelioOTAXmlResponse> UpdateRoomQuotasByOTAMessage(OTA_HotelAvailNotifRQ message)
    {
        var response = new OTA_HotelAvailNotifRS()
        {
            Success = "",
            Errors = [],
            Warnings = [],
        };

        try
        {
            if (message == null)
            {
                throw new Exception("Error processing message.");
            }

            var roomQuotas = new List<BookingRoomQuota>();

            if (message.AvailStatusMessages == null)
            {
                throw new Exception("Invalid message: No hotel or quota data.");
            }

            var roomTypeDict = await GetBookingRoomTypes(message.AvailStatusMessages.HotelCode);

            foreach (var availStatusMessage in message.AvailStatusMessages.Messages)
            {
                var date = availStatusMessage.StatusApplicationControl.Start;

                try
                {
                    while (date <= availStatusMessage.StatusApplicationControl.End)
                    {
                        var roomQuota = await CreateBookingRoomQuota(
                            roomTypeDict,
                            message.AvailStatusMessages.HotelCode,
                            availStatusMessage.StatusApplicationControl.InvTypeCode,
                            date,
                            availStatusMessage.BookingLimit);

                        roomQuotas.Add(roomQuota);

                        date = date.AddDays(1);
                    }
                }
                catch (Exception ex)
                {
                    var errorMessage = $@"HotelCode: {message.AvailStatusMessages.HotelCode}. 
                        RoomType: {availStatusMessage.StatusApplicationControl.InvTypeCode}. 
                        Date: {date}. 
                        BookingLimit: {availStatusMessage.BookingLimit}. 
                        ERROR: {ex.Message}";

                    _logService.Log(LogLevel.Error, errorMessage);
                    response.Warnings.Add(new Warning(errorMessage));
                }
            }

            _logService.Log(LogLevel.Information, $"Successfully matched {roomQuotas.Count} quotas for hotel code: {message.AvailStatusMessages.HotelCode}."); ;

            if (roomQuotas.Any())
            {
                await _bookingService.UpdateAmountAsuRoomQuotas(roomQuotas);
            }
        }
        catch (Exception ex)
        {
            _logService.Log(LogLevel.Error, ex.Message);
            response.Errors.Add(new Error(ex.Message));
        }

        return response;
    }

    public async Task<BookingRoomQuota> CreateBookingRoomQuota(
        Dictionary<string, BookingRoomTypes> roomTypeDict,
        string hotelCode,
        string roomTypeCode,
        DateTime date,
        int available,
        int reserved = 0)
    {
        if (!roomTypeDict.TryGetValue(roomTypeCode, out var roomType))
        {
            var errorMessage = $"Room type: {roomTypeCode} not found in database for hotel code: {hotelCode}.";
            _logService.Log(LogLevel.Error, errorMessage);
            throw new Exception(errorMessage);
        }

        long? amount = null;

        if (roomType.QuotasFromAsu)
        {
            var reservedKp = await _bookingService.GetReservedKpCount(roomType, date);
            amount = available + reservedKp;
        }

        return new BookingRoomQuota
        {
            HotelId = roomType.HotelId,
            RoomTypeId = roomType.Id,
            Date = date,
            Amount = amount,
            AmountAsu = available,
            ReservedAsu = reserved
        };
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
        _logService.Log(LogLevel.Information, "Try get booking room types.");

        var hotel = await _bookingService.GetHotel(hotelCode);
        if (hotel == null)
        {
            _logService.Log(LogLevel.Warning, $"Hotel with code: {hotelCode} not found in database.");
            return new Dictionary<string, BookingRoomTypes>();
        }

        var roomTypes = await _bookingService.GetRoomTypes(hotel.Id);

        _logService.Log(LogLevel.Information, $"{roomTypes.Count} booking room types got.");

        return roomTypes?.ToDictionary(key => key.HrsCode, value => value);
    }

}