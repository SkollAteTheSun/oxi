using Asp.Versioning;
using Kp.Ms.Oxi.Models.Enums;
using Kp.Ms.Oxi.Models.Request;
using Kp.Ms.Oxi.Models.Response;
using Kp.Ms.Oxi.Services;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Kp.Ms.Oxi.Controllers.V1;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
[SwaggerTag("Брони")]
public class ReservationController : ControllerBase
{
    private readonly ReservationService _reservationService;

    public ReservationController(ReservationService reservationService)
    {
        _reservationService = reservationService;
    }

    [HttpPost]
    [SwaggerOperation(Summary = "Добавление / обновление брони")]
    public async Task<ActionResult<StatusDto>> Create([FromBody] SendReservationRequest request)
    {
        string response = await _reservationService.Send(request);
        return Ok(new StatusDto
        {
            Status = StatusType.Success.ToString().ToLower(),
            Message = response
        });
    }
}