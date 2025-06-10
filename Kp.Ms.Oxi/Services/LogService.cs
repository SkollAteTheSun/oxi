using Kp.Ms.Oxi.Models.Options;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OpenSearch.Client;
using System.Xml;
using LogLevel = Microsoft.Extensions.Logging.LogLevel;

namespace Kp.Ms.Oxi.Services;

public class LogService
{
    private readonly OpenSearchClient _openSearchClient;
    private readonly OpenSearchOptions _openSearchOptions;
    private readonly ILogger<LogService> _logger;

    public LogService(OpenSearchClient openSearchClient, IOptions<OpenSearchOptions> openSearchOptions,
        ILogger<LogService> logger)
    {
        _openSearchClient = openSearchClient;
        _logger = logger;
        _openSearchOptions = openSearchOptions.Value;
    }

    public void LogOperaMessage(LogLevel logLevel, string message)
    {
        try
        {
            _logger.Log(logLevel, "{Message}", message);

            JObject json = JObject.Parse(message);
            var label = json.SelectToken("?Label");
            string[] labelParts = [];
            if (label != null)
            {
                labelParts = label.ToString().Split('|');
            }

            _openSearchClient.Index(new
            {
                date = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss"),
                level = logLevel,
                propertyName = labelParts.ElementAtOrDefault(0),
                messageType = labelParts.ElementAtOrDefault(1),
                transactionId = labelParts.ElementAtOrDefault(2),
                status = labelParts.ElementAtOrDefault(3),
                message = JsonConvert.DeserializeObject(message)
            }, idx => idx.Index(_openSearchOptions.LogIndexOperaMessages));
        }
        catch (Exception ex)
        {
            _logger.LogError("OpenSearch exception: {ExMessage}", ex.Message);
        }
    }

    public void Log(LogLevel logLevel, string message)
    {
        try
        {
            _logger.Log(logLevel, "{Message}", message);

            _openSearchClient.Index(new
            {
                date = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss"),
                message = message,
                level = logLevel
            }, idx => idx.Index(_openSearchOptions.LogIndex));
        }
        catch (Exception ex)
        {
            _logger.LogError("OpenSearch exception: {ExMessage}", ex.Message);
        }
    }

    public void LogRawOperaMessage(LogLevel logLevel, string rawXml)
    {

        if (!_openSearchOptions.EnableRawMessagesLogging)
            return;

        try
        {
            _logger.Log(logLevel, "{Message}", rawXml);

            XmlDocument xml = new XmlDocument();
            xml.LoadXml(rawXml);

            string? label = xml.LastChild?.PreviousSibling?.Value;
            string[] labelParts = Array.Empty<string>();
            if (!string.IsNullOrEmpty(label))
            {
                labelParts = label.Split('|');
            }

            _openSearchClient.Index(new
            {
                date = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss"),
                level = logLevel,
                propertyName = labelParts.ElementAtOrDefault(0),
                messageType = labelParts.ElementAtOrDefault(1),
                transactionId = labelParts.ElementAtOrDefault(2),
                status = labelParts.ElementAtOrDefault(3),
                message = rawXml,
            }, idx => idx.Index(_openSearchOptions.LogIndexOperaRawMessages));
        }
        catch (Exception ex)
        {
            _logger.LogError("OpenSearch logging error: {ExMessage}", ex.Message);
        }
    }
}