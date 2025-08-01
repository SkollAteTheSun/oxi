using System.Text;
using System.Text.Json;
using System.Xml;
using System.Xml.Serialization;
using Kp.Ms.Oxi.Common;
using Kp.Ms.Oxi.Context;
using Kp.Ms.Oxi.Models.Entity;
using Kp.Ms.Oxi.Models.Enums;
using Kp.Ms.Oxi.Models.Fidelio;
using Kp.Ms.Oxi.Models.Opera;
using Kp.Ms.Oxi.Models.Request;
using Kp.Ms.Oxi.Models.Options;
using Microsoft.AspNetCore.Http.Extensions;
using Field = Kp.Ms.Oxi.Models.Fidelio.Field;
using Profile = Kp.Ms.Oxi.Models.Opera.Profile;
using TimeUnit = Kp.Ms.Oxi.Models.Opera.TimeUnit;
using Microsoft.Extensions.Options;

namespace Kp.Ms.Oxi.Services;

public class ReservationService
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<ReservationService> _logger;
    private readonly DefaultContext _context;
    private readonly XmlCreationService _xmlCreationService;

    public ReservationService(IConfiguration configuration, ILogger<ReservationService> logger, DefaultContext context, XmlCreationService xmlCreationService)
    {
        _configuration = configuration;
        _logger = logger;
        _context = context;
        _xmlCreationService = xmlCreationService;
    }

    public async Task<string> Send(SendReservationRequest request)
    {
        return request.Interface.Type switch
        {
            InterfaceType.Oxi => await SendToOxi(request),
            InterfaceType.Fidelio => await SendToFidelio(request),
            _ => throw new Exception("Неизвестный тип интерфейса")
        };
    }

    private async Task<string> SendToOxi(SendReservationRequest request)
    {
        var xml = _xmlCreationService.CreateOxiReservationXml(request);

        string? apiResponse;

        var queryParams = new Dictionary<string, string>
        {
            { "interfaceName", request.Interface.InterfaceName },
            { "propertyName", request.Interface.HotelCode },
            { "messageType", "RESERVATION" },
            { "transactionId", request.Interface.TransactionId },
            { "status", request.Interface.Status },
        };
        var queryBuilder = new QueryBuilder(queryParams);
        string basePath = request.Interface.Url;

        bool directMode = Convert.ToBoolean(_configuration["DirectMode"]);
        if (directMode)
        {
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Clear();
                httpClient.DefaultRequestHeaders.ConnectionClose = true;
                httpClient.Timeout = TimeSpan.FromSeconds(10);

                var requestMessage = new HttpRequestMessage(HttpMethod.Post, basePath + queryBuilder);
                requestMessage.Content = new StringContent(xml, Encoding.UTF8, "text/xml");
                using (var response = httpClient.SendAsync(requestMessage).Result)
                {
                    response.EnsureSuccessStatusCode();
                    apiResponse = response.Content.ReadAsStringAsync().Result;
                }
            }
        }
        else
        {
            _context.Transactions.Add(new Transaction
            {
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                HotelCode = request.Interface.HotelCode,
                TransactionId = request.Interface.TransactionId,
                InterfaceName = request.Interface.InterfaceName,
                InterfaceType = (int)request.Interface.Type,
                Status = request.Interface.Status,
                Request = JsonSerializer.Serialize(request),
                Url = basePath + queryBuilder,
                Xml = xml
            });
            await _context.SaveChangesAsync();

            apiResponse = "saved to database";
        }

        return apiResponse;
    }
    
    //Новый метод "Отправлять новый профиль одновременно с бронью"
    private async Task<string> SendToFidelio(SendReservationRequest request)
    {
        string? apiResponse;

        if (request.Interface.Auth == null)
        {
            throw new Exception("Interface auth data is not set");
        }

        var xml = _xmlCreationService.CreateFidelioReservationXml(request);

        bool directMode = Convert.ToBoolean(_configuration["DirectMode"]);
        if (directMode)
        {
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Clear();
                httpClient.DefaultRequestHeaders.ConnectionClose = true;
                httpClient.Timeout = TimeSpan.FromSeconds(10);

                var requestMessage = new HttpRequestMessage(HttpMethod.Post, request.Interface.Url);
                requestMessage.Content = new StringContent(xml, Encoding.UTF8, "text/xml");
                using (var response = httpClient.SendAsync(requestMessage).Result)
                {
                    response.EnsureSuccessStatusCode();
                    apiResponse = response.Content.ReadAsStringAsync().Result;
                }
            }
        }
        else
        {
            _context.Transactions.Add(new Transaction
            {
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                HotelCode = request.Interface.HotelCode,
                TransactionId = request.Interface.TransactionId,
                InterfaceName = request.Interface.InterfaceName,
                InterfaceType = (int)request.Interface.Type,
                Status = request.Interface.Status,
                Request = JsonSerializer.Serialize(request),
                Url = request.Interface.Url,
                Xml = xml,
                Pre = false   // лишний флаг, можно убирать
            });
            await _context.SaveChangesAsync();

            apiResponse = "saved to database";
        }

        return apiResponse;
    }
}