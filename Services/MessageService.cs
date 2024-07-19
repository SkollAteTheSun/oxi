using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Xml;
using Kp.Ms.Oxi.Context;
using Kp.Ms.Oxi.Models.Entity;
using Kp.Ms.Oxi.Models.Request;
using Microsoft.EntityFrameworkCore;

namespace Kp.Ms.Oxi.Services;

public class MessageService
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<MessageService> _logger;
    private readonly DefaultContext _context;

    public MessageService(IConfiguration configuration, ILogger<MessageService> logger, DefaultContext context)
    {
        _configuration = configuration;
        _logger = logger;
        _context = context;
    }

    public async Task<bool> Receive(string message)
    {
        _logger.LogInformation("Input message: \n{Message}", message);

        XmlDocument xml = new XmlDocument();
        try
        {
            xml.LoadXml(message);
        }
        catch (Exception ex)
        {
            _logger.LogError("Xml parse error: {ErrorMessage}", ex.Message);
            return false;
        }

        if (string.IsNullOrEmpty(xml.LastChild?.PreviousSibling?.Value))
        {
            _logger.LogError("Label not exists");
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
            _logger.LogError("Configuration error: {Error}", ex.Message);
            return false;
        }

        string transactionId = labelParts[2];

        TransactionGetResponseRequest request = new TransactionGetResponseRequest
        {
            Token = token,
            TransactionId = transactionId,
            Status = labelParts[3] == "SUCCESS" ? "success" : "fail",
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

        _logger.LogInformation("Request to booking api transaction get response: {Request}", JsonSerializer.Serialize(request));

        var requestMessage = new HttpRequestMessage(HttpMethod.Post, baseUrl + transactionGetResponseUrl);
        requestMessage.Content = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");
        using var response = await httpClient.SendAsync(requestMessage);
        string apiResponse = await response.Content.ReadAsStringAsync();

        _logger.LogInformation("Response of booking api transaction get response: {Response}", Regex.Unescape(apiResponse));

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