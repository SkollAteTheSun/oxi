using Asp.Versioning;
using Kp.Ms.Oxi.Attributes;
using Kp.Ms.Oxi.Converters;
using Kp.Ms.Oxi.Models.Entity;
using Kp.Ms.Oxi.Models.Response;
using Kp.Ms.Oxi.Services;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Kp.Ms.Oxi.Controllers.V1;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
[SwaggerTag("Транзакции")]
public class TransactionController : ControllerBase
{
    private readonly TransactionService _transactionService;

    public TransactionController(TransactionService transactionService)
    {
        _transactionService = transactionService;
    }

    [HttpGet]
    [AuthToken]
    [SwaggerOperation(Summary = "Получение транзакции")]
    public async Task<ActionResult<TransactionDto>> GetTransaction()
    {
        Transaction? transaction = await _transactionService.FirstOrDefault();
        if (transaction == null)
        {
            return NoContent();
        }

        TransactionDto dto = TransactionConverter.Convert(transaction);

        return Ok(dto);
    }
}