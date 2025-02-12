using Kp.Ms.Oxi.Models.Entity;
using Kp.Ms.Oxi.Models.Opera;
using System.Text.Json;
using System.Xml;
using System.Xml.Serialization;

namespace Kp.Ms.Oxi.Services;

public class QuotaService
{
    private readonly LogService _logService;
    public QuotaService(LogService logService)
    {
        _logService = logService;
    }

    /*
    public string ConvertXmlToJson(string xmlInput)
    {
        XmlSerializer serializer = new XmlSerializer(typeof(RtavMessage));

        using StringReader reader = new StringReader(xmlInput);
        var deserializedObject = (RtavMessage?)serializer.Deserialize(reader);

        if (deserializedObject == null)
        {
            throw new InvalidOperationException("Ошибка десериализации XML.");
        }

        return JsonSerializer.Serialize(deserializedObject, new JsonSerializerOptions
        {
            WriteIndented = true
        });
    }
    */

    public async Task<string?> ConvertXmlToJson(XmlDocument xmlDocument)
    {
        string? serializedResult = null; // Переменная для хранения сериализованного результата

        try
        {
            var rootNode = xmlDocument.DocumentElement;
            if (rootNode == null)
            {
                //_logService.Log(LogLevel.Error, "XML не содержит корневого элемента.");
                return null;
            }

            XmlSerializer serializer = new XmlSerializer(typeof(RtavMessage));
            using StringReader reader = new StringReader(rootNode.OuterXml);

            var deserializedObject = (RtavMessage?)serializer.Deserialize(reader);
            if (deserializedObject == null)
            {
                //_logService.Log(LogLevel.Error, "Ошибка десериализации XML.");
                return null;
            }

            serializedResult = JsonSerializer.Serialize(deserializedObject, new JsonSerializerOptions
            {
                WriteIndented = true
            });

            //_logService.Log(LogLevel.Information, $"Сериализованный JSON: {serializedResult}");
        }
        catch (Exception ex)
        {
            //_logService.Log(LogLevel.Error, $"Ошибка обработки XML: {ex.Message}");
            return null;
        }

        return serializedResult;
    }

    /*
    return JsonSerializer.Serialize(deserializedObject, new JsonSerializerOptions
    {
        WriteIndented = true
    });
    */

    public List<RoomQuota> MapToRoomQuotas(RtavMessage message)
    {
        var roomQuotas = new List<RoomQuota>();

        if (message.HotelReference == null || message.DailyInventories == null)
            return roomQuotas;

        foreach (var inventory in message.DailyInventories)
        {
            if (inventory.RoomTypeInventories == null) continue;

            foreach (var roomTypeInventory in inventory.RoomTypeInventories)
            {
                var roomQuota = new RoomQuota
                {
                    HotelId = long.Parse(message.HotelReference.HotelCode), // Тут вообще надо маппить Opera -> Booking Id
                    RoomTypeId = long.Parse(roomTypeInventory.RoomType),
                    Date = inventory.Datum,
                    Amount = roomTypeInventory.Available,
                    Close = roomTypeInventory.Available <= 0 // тут типа если количество доступных комнат <= 0, то закрываем квоту?
                };

                roomQuotas.Add(roomQuota);
            }
        }

        return roomQuotas;
    }

    public async Task<RtavMessage?> DeserializeXmlToMessage(XmlDocument xmlDocument)
    {
        try
        {
            var rootNode = xmlDocument.DocumentElement;
            if (rootNode == null) return null;

            XmlSerializer serializer = new XmlSerializer(typeof(RtavMessage));
            using StringReader reader = new StringReader(rootNode.OuterXml);

            return (RtavMessage?)serializer.Deserialize(reader);
        }
        catch (Exception ex)
        {
            //_logService.Log(LogLevel.Error, $"Ошибка десериализации XML: {ex.Message}");
            return null;
        }
    }

}