using Asp.Versioning;
using Kp.Ms.Oxi.Services;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Kp.Ms.Oxi.Controllers.V1;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
[SwaggerTag("Контроллер обработки входящих сообщений от внешних систем")]
public class InputController : ControllerBase
{
    private readonly InputService _inputService;

    public InputController(InputService inputService)
    {
        _inputService = inputService;
    }

    [HttpPost("receive")]
    [SwaggerOperation(Summary = "Получение сообщения")]
    public async Task<ActionResult> Receive()
    {
        using StreamReader reader = new(Request.Body);
        string message = await reader.ReadToEndAsync();
        await _inputService.Receive(message);
        return Ok();
    }
}