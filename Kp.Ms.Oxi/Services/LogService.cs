using Kp.Ms.Oxi.Models.Options;
using Microsoft.Extensions.Options;
using OpenSearch.Client;
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
}