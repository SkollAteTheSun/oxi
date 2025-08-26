using Kp.Ms.Oxi.Config;
using Microsoft.Extensions.Options;

namespace Kp.Ms.Oxi.Services;

public class CheckOutResendHostedService : IHostedService, IDisposable
{
    private readonly ILogger<CheckOutResendHostedService> _logger;
    private readonly PmsService _pmsService;
    private readonly ResendCheckOutsSettings _settings;

    private const int DEFAULT_TIMEOUT_MINUTES = 1440;

    private Timer _timer;

    public CheckOutResendHostedService(
        ILogger<CheckOutResendHostedService> logger,
        PmsService pmsService,
        IOptions<ResendCheckOutsSettings> resendCheckOutsSettings)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _pmsService = pmsService ?? throw new ArgumentNullException(nameof(pmsService));
        _settings = resendCheckOutsSettings?.Value;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("The CheckOutResendHostedService started.");

        _timer = new Timer(async (state) => await DoWork(state), null, TimeSpan.Zero,
            TimeSpan.FromMinutes(_settings.ResendTimeoutMinutes > 0 ? _settings.ResendTimeoutMinutes : DEFAULT_TIMEOUT_MINUTES));

        return Task.CompletedTask;
    }

    private async Task DoWork(object state)
    {
        try
        {
            await _pmsService.ResendCheckOuts();
        }
        catch (Exception ex)
        {
            _logger.LogInformation($"Resend check-outs failed: {ex.Message}");
        }
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("The CheckOutResendHostedService stopped.");

        _timer?.Change(Timeout.Infinite, 0);

        return Task.CompletedTask;
    }

    public void Dispose()
    {
        _timer?.Dispose();
    }
}
