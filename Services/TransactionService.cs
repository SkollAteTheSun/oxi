using System.Text.Encodings.Web;
using System.Text.Json;
using Kp.Ms.Oxi.Context;
using Kp.Ms.Oxi.Models.Entity;
using Microsoft.EntityFrameworkCore;

namespace Kp.Ms.Oxi.Services;

public class TransactionService
{
    private readonly LogService _logService;
    private readonly DefaultContext _context;

    public TransactionService(DefaultContext context, LogService logService)
    {
        _context = context;
        _logService = logService;
    }

    public async Task<Transaction?> FirstOrDefault()
    {
        DateTimeOffset minus1Hour = new DateTimeOffset(DateTime.UtcNow.AddHours(-1));
        Transaction? transaction = await _context.Transactions
            .Where(t => t.Sended == false || (t.Sended && t.SendedAt < minus1Hour))
            .Where(t => t.Received == false)
            .OrderBy(t => t.Id)
            .FirstOrDefaultAsync();

        if (transaction == null) return null;

        transaction.Sended = true;
        transaction.SendedAt = DateTime.UtcNow;
        transaction.UpdatedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();

        JsonSerializerOptions jso = new JsonSerializerOptions
        {
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
        };
        _logService.Log(LogLevel.Information, $"Input message: \n{JsonSerializer.Serialize(transaction, jso)}");

        return transaction;
    }
}