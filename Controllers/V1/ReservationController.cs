using Asp.Versioning;
using Kp.Ms.Oxi.Models.Request;
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
    [SwaggerOperation(Summary = "Создание новой брони в опере")]
    public async Task<ActionResult<string>> Create([FromBody] CreateReservationRequest request)
    {
        string response = await _reservationService.Send(request);
        return Ok(response);
    }
}