using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Xml;
using Kp.Ms.Oxi.Context;
using Kp.Ms.Oxi.Models.Entity;
using Kp.Ms.Oxi.Models.Request;
using Microsoft.EntityFrameworkCore;
using LogLevel = Microsoft.Extensions.Logging.LogLevel;

namespace Kp.Ms.Oxi.Services;

public class MessageService
{
    private readonly IConfiguration _configuration;
    private readonly DefaultContext _context;
    private readonly LogService _logService;

    public MessageService(IConfiguration configuration, DefaultContext context, LogService logService)
    {
        _configuration = configuration;
        _context = context;
        _logService = logService;
    }

    public async Task<bool> Receive(string message, MessageReceiveRequest? messageReceiveRequest)
    {
        _logService.Log(LogLevel.Information, $"Input message: \n{message}");

        JsonSerializerOptions jso = new JsonSerializerOptions
        {
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
        };

        if (messageReceiveRequest != null)
        {
            _logService.Log(LogLevel.Information, $"Query params: \n{JsonSerializer.Serialize(messageReceiveRequest, jso)}");
        }

        XmlDocument xml = new XmlDocument();
        try
        {
            xml.LoadXml(message);
        }
        catch (Exception ex)
        {
            _logService.Log(LogLevel.Error, $"Xml parse error: {ex.Message}");
            return false;
        }

        if (string.IsNullOrEmpty(xml.LastChild?.PreviousSibling?.Value))
        {
            _logService.Log(LogLevel.Error, "Label not exists");
            return false;
        }

        string label = xml.LastChild?.PreviousSibling?.Value!;
        string[] labelParts = label.Split('|');

        using var httpClient = new HttpClient();
        httpClient.DefaultRequestHeaders.Clear();
        httpClient.DefaultRequestHeaders.ConnectionClose = true;
        httpClient.Timeout = TimeSpan.FromSeconds(10);

        string baseUrl;
        string token;
        string transactionGetResponseUrl;
        try
        {
            baseUrl = _configuration["BookingApi:BaseUrl"] ??
                      throw new Exception("Booking api base url not configured");
            token = _configuration["BookingApi:Token"] ??
                    throw new Exception("Booking api token not configured");
            transactionGetResponseUrl = _configuration["BookingApi:TransactionGetResponseUrl"] ??
                                        throw new Exception("Booking api transaction get response not configured");
        }
        catch (Exception ex)
        {
            _logService.Log(LogLevel.Error, $"Configuration error: {ex.Message}");
            return false;
        }

        string transactionId = labelParts[2];
        if (!transactionId.Contains("BK"))
        {
            return false;
        }
        string transactionIdExternal = transactionId.Substring(2);

        string statusFromOpera;
        switch (labelParts[3])
        {
            case "SUCCESS":
            case "ERROR":
                statusFromOpera = labelParts[3].ToLower();
                break;
            case "FAILED":
                statusFromOpera = "fail";
                break;
            default:
                statusFromOpera = "error";
                break;
        }

        TransactionGetResponseRequest request = new TransactionGetResponseRequest
        {
            Token = token,
            TransactionId = transactionIdExternal,
            Status = statusFromOpera,
            Response = message
        };

        // если мы пытаемся апдейтить бронь которой нет в опере, повторно такие попытки делать не надо, поэтому статус error
        // TODO сделать enum на статусы для второго апи
        if (message.Contains("Reservation not found for External system"))
        {
            request.Status = "error";
        }

        XmlElement? xRoot = xml.DocumentElement;
        if (xRoot != null)
        {
            XmlNode? attr = xRoot.Attributes.GetNamedItem("success");
            if (attr?.Value == "SUCCESS")
            {
                foreach (XmlElement xnode in xRoot)
                {
                    if (xnode.Name != "ResultIds") continue;

                    foreach (XmlNode resultId in xnode.ChildNodes)
                    {
                        XmlNode? resultMsgType = resultId.Attributes?.GetNamedItem("resultMsgType");
                        foreach (XmlNode resultIdChild in resultId.ChildNodes)
                        {
                            // skip travel agent
                            if (resultIdChild.Name == "crsId" && resultIdChild.InnerText == "1")
                            {
                                break;
                            }

                            if (resultIdChild.Name == "pmsId")
                            {
                                switch (resultMsgType?.Value)
                                {
                                    case "PROFILE":
                                        request.GuestExternalId = resultIdChild.InnerText;
                                        break;
                                    case "RESERVATION":
                                        request.ReservationExternalId = resultIdChild.InnerText;
                                        break;
                                }
                            }
                        }
                    }
                }
            }
        }

        _logService.Log(LogLevel.Information, $"Request to booking api transaction get response: {JsonSerializer.Serialize(request, jso)}");

        var requestMessage = new HttpRequestMessage(HttpMethod.Post, baseUrl + transactionGetResponseUrl);
        requestMessage.Content = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");
        using var response = await httpClient.SendAsync(requestMessage);
        string apiResponse = await response.Content.ReadAsStringAsync();

        _logService.Log(LogLevel.Information, $"Response of booking api transaction get response: {Regex.Unescape(apiResponse)}");

        Transaction? transaction = await _context.Transactions.FirstOrDefaultAsync(t => t.TransactionId == transactionId);
        if (transaction != null)
        {
            transaction.Received = true;
            transaction.ReceivedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
        }

        return true;
    }
}