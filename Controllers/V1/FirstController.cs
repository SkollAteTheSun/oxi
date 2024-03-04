using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Kp.Ms.Oxi.Controllers.V1;

[ApiController]
[ApiVersion("1.0")]
[Route("api/oxi/v{version:apiVersion}/[controller]")]
[SwaggerTag("Тестовый контроллер")]
public class FirstController : ControllerBase
{
    private readonly ILogger<FirstController> _logger;

    public FirstController(ILogger<FirstController> logger)
    {
        _logger = logger;
    }
    
    [HttpPost("post")]
    [SwaggerOperation(Summary = "Получение тестовой информации")]
    public async Task<ActionResult> GetTest()
    {
        using StreamReader reader = new(Request.Body);
        string bodyAsString = await reader.ReadToEndAsync();
        _logger.LogWarning("Oxi post body: \n{BodyAsString}", bodyAsString);
        return Ok();
    }
}