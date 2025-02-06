using Asp.Versioning;
using Kp.Ms.Oxi.Models.Entity;
using Kp.Ms.Oxi.Models.Request;
using Kp.Ms.Oxi.Services;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Kp.Ms.Oxi.Controllers;

[ApiController]
[ApiVersion("2.0")]
[Route("api/v{version:apiVersion}/pms")]
[SwaggerTag("Выезды")]
public class PmsController : BaseController
{
    private readonly PmsService _pmsService;

    public PmsController(
        ILogger<PmsController> logger,
        PmsService pmsService) : base(logger)
    {
        _pmsService = pmsService ?? throw new ArgumentNullException(nameof(pmsService));
    }

    [HttpPost("check-outs")]
    [SwaggerOperation(Summary = "Прием и обработка выездов")]
    public async ValueTask<ActionResult> CheckOuts([FromBody] IEnumerable<CheckOutInfo> checkouts)
    {
        var result = await ExecuteRequestAsync(checkouts, async () =>
            await _pmsService.AddCheckOutTasksAsync(checkouts));

        return Ok(result);
    }

    [HttpGet("hotels")]
    [SwaggerOperation(Summary = "Отели для которых обрабатываеются выезды")]
    public async ValueTask<ActionResult> GetHotels()
    {
        var result = ExecuteRequest<object, IEnumerable<Hotel>>(null, () =>
            _pmsService.GetHotels());

        return Ok(result);
    }

    [HttpPost("hotels")]
    [SwaggerOperation(Summary = "Прием отеля на обработку выездов")]
    public async ValueTask<ActionResult> AddHotel([FromBody] HotelContract hotel)
    {
        var result = await ExecuteRequestAsync(hotel, async () =>
            await _pmsService.AddHotel(hotel));

        return Ok(result);
    }
}
