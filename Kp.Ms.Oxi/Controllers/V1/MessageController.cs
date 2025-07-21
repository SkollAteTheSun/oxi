using Asp.Versioning;
using Kp.Ms.Oxi.Models.Fidelio.Response;
using Kp.Ms.Oxi.Models.Request;
using Kp.Ms.Oxi.Models.Response;
using Kp.Ms.Oxi.Services;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Kp.Ms.Oxi.Controllers.V1;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
[SwaggerTag("Сообщения от внешних систем")]
public class MessageController : BaseController
{
    private readonly MessageService _messageService;

    public MessageController(
        ILogger<MessageController> logger,
        MessageService messageService) : base(logger)
    {
        _messageService = messageService;
    }

    [HttpPost("receive")]
    [SwaggerOperation(Summary = "Получение сообщения от oxi и отправка в букинг")]
    public async Task<ActionResult> Receive([FromQuery] MessageReceiveRequest? messageReceiveRequest)
    {
        using StreamReader reader = new(Request.Body);
        string message = await reader.ReadToEndAsync();
        await _messageService.Receive(message, messageReceiveRequest);
        return Ok();
    }

    [HttpPost("receive/fidelio")]
    [Consumes("application/xml")]
    [SwaggerOperation(Summary = "Получение сообщения от fidelio и отправка в букинг")]
    public async Task<ActionResult<StatusDto>> ReceiveFidelio()
    {
        using StreamReader reader = new(Request.Body);
        string message = await reader.ReadToEndAsync();
        StatusDto dto = await _messageService.FidelioResult(message);

        return Ok(dto);
    }

    [HttpPost("receive/fidelio-xml")]
    [SwaggerOperation(Summary = "Получение сообщения от fidelio и отправка в букинг")]
    [Produces("application/xml")]
    public async Task<ActionResult<MessageReceiveXmlDto>> ReceiveFidelioXml()
    {
        using StreamReader reader = new(Request.Body);
        string message = await reader.ReadToEndAsync();
        var xml = await _messageService.FidelioMessageXml(message);

        return Ok(xml);
    }
}