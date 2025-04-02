using Asp.Versioning;
using Kp.Ms.Oxi.Attributes;
using Kp.Ms.Oxi.Converters;
using Kp.Ms.Oxi.Models.Entity;
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
    [SwaggerOperation(Summary = "Получение транзакций")]
    public async Task<ActionResult<List<TransactionDto>>> GetTransactions([FromQuery] TransactionRequest request)
    {
        List<Transaction> transactions = await _transactionService.TakeAsync(request);
        List<TransactionDto> dtos = transactions.Select(TransactionConverter.Convert).ToList();

        return Ok(dtos);
    }

    [HttpPost("acknowledge")]
    [SwaggerOperation(Summary = "Подтверждение передачи транзакций в PMS")]
    public async Task<ActionResult<List<long>>> AcknowledgeTransaction([FromBody] List<long> request)
    {
        var transactions = await _transactionService.AcknowledgeAsync(request);

        return Ok(transactions);
    }

    [HttpGet("one")]
    [SwaggerOperation(Summary = "Получение одной транзакции")]
    public async Task<ActionResult<string>> GetTransaction([FromQuery] TransactionOneRequest request)
    {
        try
        {
            Transaction? transaction = await _transactionService.TakeFirstAsync(request);
            if (transaction == null)
            {
                return NoContent();
            }

            return Ok(transaction.Xml);
        }
        catch (Exception ex)
        {
            return BadRequest(new StatusDto
            {
                Status = StatusType.Fail.ToString().ToLower(),
                Message = ex.Message
            });
        }
    }
}