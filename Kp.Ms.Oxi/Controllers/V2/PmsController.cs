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

    [HttpPost("check-outs/booking")]
    [SwaggerOperation(Summary = "Отправка выездов в букинг")]
    public async ValueTask<ActionResult> SendCheckOutsToBooking([FromBody] List<CheckOutToBookingContract> checkOuts)
    {
        var result = await ExecuteRequestAsync(checkOuts, async () =>
        {
            var results = new List<object>();
            foreach (var checkOut in checkOuts) {
                var message = "success";
                try
                {
                    await _pmsService.SendToBooking(null, checkOut.Number, checkOut.Id, checkOut.CheckOutDate);
                }
                catch (Exception ex)
                {
                    message = ex.Message;
                }

                results.Add(new
                {
                    id = checkOut.Id,
                    message = message
                });
            }

            return results;
        });

        return Ok(result);
    }

    [HttpPost("check-outs/shop-status")]
    [SwaggerOperation(Summary = "Отправка выездов в шоп (статус)")]
    public async ValueTask<ActionResult> SendCheckOutsToShopStatus([FromBody] List<CheckOutToShopStatusContract> checkOuts)
    {
        var result = await ExecuteRequestAsync(checkOuts, async () =>
        {
            var results = new List<object>();
            foreach (var checkOut in checkOuts)
            {
                var message = "success";
                try
                {
                    await _pmsService.EditReservationStatusInShop(null, checkOut.Number, checkOut.Id);
                }
                catch (Exception ex)
                {
                    message = ex.Message;
                }

                results.Add(new
                {
                    id = checkOut.Id,
                    message = message
                });
            }

            return results;
        });

        return Ok(result);
    }

    [HttpPost("check-outs/shop-full-payment")]
    [SwaggerOperation(Summary = "Отправка выездов в шоп (оплата)")]
    public async ValueTask<ActionResult> SendCheckOutsToShopFullPayment([FromBody] List<CheckOutToShopFullPaymentContract> checkOuts)
    {
        var result = await ExecuteRequestAsync(checkOuts, async () =>
        {
            var results = new List<object>();
            foreach (var checkOut in checkOuts)
            {
                var message = "success";
                try
                {
                    await _pmsService.SendFullPaymentReceiptInShop(null, checkOut.Number, checkOut.OrderId);
                }
                catch (Exception ex)
                {
                    message = ex.Message;
                }

                results.Add(new
                {
                    id = checkOut.OrderId,
                    message = message
                });
            }

            return results;
        });

        return Ok(result);
    }

    [HttpPost("check-outs/mindbox")]
    [SwaggerOperation(Summary = "Отправка выездов в mindbox")]
    public async ValueTask<ActionResult> SendCheckOutsToMindbox([FromBody] List<CheckOutToMindboxContract> checkOuts)
    {
        var result = await ExecuteRequestAsync(checkOuts, async () =>
        {
            var results = new List<object>();
            foreach (var checkOut in checkOuts)
            {
                var message = "success";
                try
                {
                    await _pmsService.SendToMindbox(null, checkOut.Number);
                }
                catch (Exception ex)
                {
                    message = ex.Message;
                }

                results.Add(new
                {
                    id = checkOut.Number,
                    message = message
                });
            }

            return results;
        });

        return Ok(result);
    }
}
