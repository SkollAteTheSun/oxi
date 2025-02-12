using Asp.Versioning;
using Kp.Ms.Oxi.Models.Request;
using Kp.Ms.Oxi.Services;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Kp.Ms.Oxi.Controllers.V1;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
[SwaggerTag("Тарифы")]
public class RateController : ControllerBase
{
    private readonly RateService _rateService;

    public RateController(RateService rateService)
    {
        _rateService = rateService;
    }

    [HttpPost("post")]
    [SwaggerOperation(Summary = "Выгрузка цен по тарифу в оперу")]
    public async Task<ActionResult> RateSet([FromBody] RateSetRequest request)
    {
        string response = await _rateService.MakeRateSet(request);
        return Ok(response);
    }
}