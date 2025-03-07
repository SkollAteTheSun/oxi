using System.Text.Encodings.Web;
using System.Text.Json;
using Kp.Ms.Oxi.Context;
using Kp.Ms.Oxi.Models.Entity;
using Kp.Ms.Oxi.Models.Request;
using Microsoft.EntityFrameworkCore;

namespace Kp.Ms.Oxi.Services;

public class TransactionService
{
    private readonly LogService _logService;
    private readonly DefaultContext _context;
    private readonly IConfiguration _configuration;
    private readonly JsonSerializerOptions _jso;

    public TransactionService(DefaultContext context, LogService logService, IConfiguration configuration)
    {
        _context = context;
        _logService = logService;
        _configuration = configuration;
        _jso = new JsonSerializerOptions
        {
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
        };
    }

    public async Task<List<Transaction>> TakeAsync(TransactionRequest request)
    {
        int batchSize = Convert.ToInt16(_configuration["TransactionBatchSize"]);

        DateTimeOffset minus1Hour = new DateTimeOffset(DateTime.UtcNow.AddHours(-1));
        List<Transaction> transactions = await _context.Transactions
            .Where(t => t.Sended == false || (t.Sended && t.SendedAt < minus1Hour))
            .Where(t => t.Received == false)
            .Where(t => t.HotelCode == request.HotelCode && t.InterfaceName == request.InterfaceName)
            .OrderBy(t => t.Id)
            .Take(batchSize)
            .ToListAsync();

        transactions.ForEach(transaction =>
        {
            transaction.Sended = true;
            transaction.SendedAt = DateTime.UtcNow;
            transaction.UpdatedAt = DateTime.UtcNow;
            _context.SaveChanges();

            _logService.Log(LogLevel.Information, $"Transaction get: \n{JsonSerializer.Serialize(transaction, _jso)}");
        });

        return transactions;
    }

    public async Task<Transaction?> TakeFirstAsync(TransactionOneRequest request)
    {
        DateTimeOffset minus1Hour = new DateTimeOffset(DateTime.UtcNow.AddHours(-1));
        Transaction? transaction = await _context.Transactions
            .Where(t => t.Sended == false || (t.Sended && t.SendedAt < minus1Hour))
            .Where(t => t.Received == false)
            .Where(t => t.HotelCode == request.PropertyName)
            .OrderBy(t => t.Id)
            .FirstOrDefaultAsync();

        if (transaction != null)
        {
            transaction.Sended = true;
            transaction.SendedAt = DateTime.UtcNow;
            transaction.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            _logService.Log(LogLevel.Information, $"Transaction get: \n{JsonSerializer.Serialize(transaction, _jso)}");
        }
        else
        {
            _logService.Log(LogLevel.Information, $"Transaction not found");
        }

        return transaction;
    }
}