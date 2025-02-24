using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Serialization;
using Kp.Ms.Oxi.Context;
using Kp.Ms.Oxi.Models.Entity;
using Kp.Ms.Oxi.Models.Enums;
using Kp.Ms.Oxi.Models.Opera;
using Kp.Ms.Oxi.Models.Request;
using Kp.Ms.Oxi.Models.Request.Fidelio.Result;
using Kp.Ms.Oxi.Models.Response;
using Microsoft.EntityFrameworkCore;
using LogLevel = Microsoft.Extensions.Logging.LogLevel;

namespace Kp.Ms.Oxi.Services;

public class MessageService
{
    private readonly IConfiguration _configuration;
    private readonly DefaultContext _context;
    private readonly LogService _logService;
    private readonly ReservationService _reservationService;
    private readonly QuotaService _quotaService;

    public MessageService(IConfiguration configuration, DefaultContext context, LogService logService,
        ReservationService reservationService, QuotaService quotaService)
    {
        _configuration = configuration;
        _context = context;
        _logService = logService;
        _reservationService = reservationService;
        _quotaService = quotaService;
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

        XmlElement? root = xml.DocumentElement;
        if (root == null)
        {
            _logService.Log(LogLevel.Error, "Root element is missing");
            return false;
        }

        if (root.Name == nameof(Reservation)) 
        {
            await _quotaService.UpdateRoomQuotasByReservation(xml);
        }

        if (root.Name == nameof(RtavMessage) && root.NamespaceURI.Contains("rtav.fidelio"))
        {
            return await _quotaService.UpdateRoomQuotasByRtavMessage(xml);
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

        List<Transaction> transactions = await _context.Transactions
            .Where(t => t.TransactionId == transactionId && t.Received == false)
            .ToListAsync();
        transactions.ForEach(t => { t.Received = true; t.ReceivedAt = DateTime.UtcNow; });
        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<StatusDto> FidelioResult(string message)
    {
        FidelioResultRequest? request;
        try
        {
            XmlSerializer serializer = new XmlSerializer(typeof(FidelioResultRequest));
            using TextReader reader = new StringReader(message);
            request = serializer.Deserialize(reader) as FidelioResultRequest;
        }
        catch (Exception ex)
        {
            StatusDto dto = new StatusDto
            {
                Status = StatusType.Fail.ToString().ToLower(),
                Message = $"Fidelio result request parse error: {ex.Message}"
            };
            _logService.Log(LogLevel.Error, dto.Message);
            return dto;
        }

        if (request == null)
        {
            StatusDto dto = new StatusDto
            {
                Status = StatusType.Fail.ToString().ToLower(),
                Message = "Fidelio result request is null"
            };
            _logService.Log(LogLevel.Error, dto.Message);
            return dto;
        }

        JsonSerializerOptions jso = new JsonSerializerOptions
        {
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
        };
        _logService.Log(LogLevel.Information, $"Fidelio result: \n{JsonSerializer.Serialize(request, jso)}");

        Transaction? transaction = await _context.Transactions.FirstOrDefaultAsync(t => t.TransactionId == request.Response.Id);
        if (transaction == null)
        {
            StatusDto dto = new StatusDto
            {
                Status = StatusType.Fail.ToString().ToLower(),
                Message = "Fidelio result request transaction ID not found in DB"
            };
            _logService.Log(LogLevel.Error, dto.Message);
            return dto;
        }

        if (transaction.Pre)
        {
            SendReservationRequest? reservationRequest = JsonSerializer.Deserialize<SendReservationRequest>(transaction.Request);
            if (reservationRequest == null)
            {
                StatusDto dto = new StatusDto
                {
                    Status = StatusType.Fail.ToString().ToLower(),
                    Message = "Dont parse reservation request in transaction"
                };
                _logService.Log(LogLevel.Error, dto.Message);
                return dto;
            }

            reservationRequest.Reservation.Profile.ExternalId = request.Response.InsertResponse?.Rows?.ProfileId;
            reservationRequest.Interface.TransactionId =
                reservationRequest.Interface.TransactionId.Substring(2, reservationRequest.Interface.TransactionId.Length - 10);
            if (string.IsNullOrEmpty(reservationRequest.Reservation.Profile.ExternalId))
            {
                StatusDto dto = new StatusDto
                {
                    Status = StatusType.Fail.ToString().ToLower(),
                    Message = "Fidelio result profile id is empty"
                };
                _logService.Log(LogLevel.Error, dto.Message);
                return dto;
            }
            await _reservationService.Send(reservationRequest);
        }
        else
        {
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
                StatusDto dto = new StatusDto
                {
                    Status = StatusType.Fail.ToString().ToLower(),
                    Message = $"Configuration error: {ex.Message}"
                };
                _logService.Log(LogLevel.Error, dto.Message);
                return dto;
            }

            string statusFromFidelio;
            switch (request.Response.Status)
            {
                case "OK":
                    statusFromFidelio = "success";
                    break;
                default:
                    statusFromFidelio = "error";
                    break;
            }

            SendReservationRequest? reservationRequest = JsonSerializer.Deserialize<SendReservationRequest>(transaction.Request);
            if (reservationRequest == null)
            {
                StatusDto dto = new StatusDto
                {
                    Status = StatusType.Fail.ToString().ToLower(),
                    Message = "Dont parse reservation request in transaction"
                };
                _logService.Log(LogLevel.Error, dto.Message);
                return dto;
            }

            TransactionGetResponseRequest requestToBk = new TransactionGetResponseRequest
            {
                Token = token,
                TransactionId = reservationRequest.Interface.TransactionId.Substring(2),
                Status = statusFromFidelio,
                Response = message,
                GuestExternalId = reservationRequest.Reservation.Profile.ExternalId
            };

            if (statusFromFidelio == "success")
            {
                if (request.Response.InsertResponse != null)
                {
                    if (request.Response.InsertResponse.Rows.GuestNum == null)
                    {
                        StatusDto dto = new StatusDto
                        {
                            Status = StatusType.Fail.ToString().ToLower(),
                            Message = "Response from fidelio dont contain GuestNum"
                        };
                        _logService.Log(LogLevel.Error, dto.Message);
                        return dto;
                    }

                    requestToBk.ReservationExternalId = request.Response.InsertResponse.Rows.GuestNum.ToString();
                }
                else if (request.Response.UpdateResponse != null || reservationRequest.Reservation.ExternalId != null)
                {
                    requestToBk.ReservationExternalId = reservationRequest.Reservation.ExternalId;
                }
            }

            _logService.Log(LogLevel.Information, $"Request to booking api transaction get response url {baseUrl + transactionGetResponseUrl} body: {JsonSerializer.Serialize(requestToBk, jso)}");

            var requestMessage = new HttpRequestMessage(HttpMethod.Post, baseUrl + transactionGetResponseUrl);
            requestMessage.Content = new StringContent(JsonSerializer.Serialize(requestToBk, jso), Encoding.UTF8, "application/json");
            using var response = await httpClient.SendAsync(requestMessage);
            string apiResponse = await response.Content.ReadAsStringAsync();

            _logService.Log(LogLevel.Information, $"Response from booking api transaction get response: {Regex.Unescape(apiResponse)}");
        }

        transaction.Received = true;
        transaction.ReceivedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();

        return new StatusDto { Status = StatusType.Success.ToString().ToLower() };
    }
}