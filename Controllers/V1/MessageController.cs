using Asp.Versioning;
using Kp.Ms.Oxi.Services;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Kp.Ms.Oxi.Controllers.V1;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
[SwaggerTag("Сообщения от внешних систем")]
public class MessageController : ControllerBase
{
    private readonly MessageService _messageService;

    public MessageController(MessageService messageService)
    {
        _messageService = messageService;
    }

    [HttpPost("receive")]
    [SwaggerOperation(Summary = "Получение сообщения")]
    public async Task<ActionResult> Receive()
    {
        using StreamReader reader = new(Request.Body);
        string message = await reader.ReadToEndAsync();
        await _messageService.Receive(message);
        return Ok();
    }
}