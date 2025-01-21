using Asp.Versioning;
using Kp.Ms.Oxi.Models.Entity;
using Kp.Ms.Oxi.Models.Request;
using Kp.Ms.Oxi.Models.Response;
using Kp.Ms.Oxi.Services;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Kp.Ms.Oxi.Controllers;

[ApiController]
[ApiVersion("2.0")]
[Route("api/v{version:apiVersion}/pms")]
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
    public async ValueTask<WebApiResponse<bool>> CheckOuts([FromBody] IEnumerable<CheckOutInfo> checkouts)
    {
        var result = await ExecuteRequestAsync(checkouts, async () =>
            await _pmsService.AddCheckOutTasksAsync(checkouts));

        Response.StatusCode = (int)HttpStatusCode.OK;

        return result;
    }

    [HttpGet("hotels")]
    public async ValueTask<WebApiResponse<IEnumerable<Hotel>>> GetHotels()
    {
        var result = ExecuteRequest<object, IEnumerable<Hotel>>(null, () =>
            _pmsService.GetHotels());

        Response.StatusCode = (int)HttpStatusCode.OK;

        return result;
    }

    [HttpPost("hotels")]
    public async ValueTask<WebApiResponse<Hotel>> AddHotel([FromBody] HotelContract hotel)
    {
        var result = await ExecuteRequestAsync(hotel, async () =>
            await _pmsService.AddHotel(hotel));

        Response.StatusCode = (int)HttpStatusCode.Created;

        return result;
    }
}
