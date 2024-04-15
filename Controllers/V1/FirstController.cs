using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Kp.Ms.Oxi.Controllers.V1;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
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

    [HttpPost("get")]
    [SwaggerOperation(Summary = "Получение тестовой информации business events")]
    public async Task<ActionResult> GetGet()
    {
        using var httpClient = new HttpClient();
        // httpClient.Timeout = TimeSpan.FromSeconds(10);
        httpClient.DefaultRequestHeaders.Clear();
        httpClient.DefaultRequestHeaders.ConnectionClose = true;

        var requestMessage = new HttpRequestMessage(HttpMethod.Get, "http://10.100.212.40/Operajserv/OXIServlets/BEInterface?subscriberName=EQC&ignoreNullValue=0&showOldValue=1");
        var response = await httpClient.SendAsync(requestMessage);
        response.EnsureSuccessStatusCode();
        var apiResponse = await response.Content.ReadAsStringAsync();
        //var t = apiResponse.ReadAsync();
        // StreamReader streamReader = new StreamReader(response.Content.ReadAsStreamAsync().Result);
        // streamReader.BaseStream.Position = 0;
        // string rawJson = streamReader.ReadToEnd();

        return Ok(apiResponse);
    }
}