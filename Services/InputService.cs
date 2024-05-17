namespace Kp.Ms.Oxi.Services;

public class InputService
{
    private readonly ILogger<InputService> _logger;

    public InputService(ILogger<InputService> logger)
    {
        _logger = logger;
    }

    public async Task Receive(string message)
    {
        _logger.LogWarning("Input message: \n{Message}", message);
    }
}