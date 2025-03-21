using System.Runtime.CompilerServices;
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
            .Where(t => t.Delivered == false)
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

    public async Task<List<long>> AcknowledgeAsync(List<long> transactionIds)
    {
        _logService.Log(LogLevel.Information, $"Transactions for acknowledge [{transactionIds.Count()}]: \n{JsonSerializer.Serialize(transactionIds, _jso)}");

        var transactions = await _context.Transactions
            .Where(t => transactionIds.Contains(t.Id))
            .ToListAsync();

        transactions.ForEach(transaction =>
        {
            transaction.Delivered = true;
            transaction.DeliveredAt = DateTime.UtcNow;
            transaction.UpdatedAt = DateTime.UtcNow;
            _context.SaveChanges();

            _logService.Log(LogLevel.Information, $"Acknowledged transaction: \n{JsonSerializer.Serialize(transaction, _jso)}");
        });

        var acknowledgedTransactions = transactions
            .Select(transaction => transaction.Id)
            .ToList();

        _logService.Log(LogLevel.Information, $"Acknowledged transactions [{acknowledgedTransactions.Count()}]: \n{JsonSerializer.Serialize(acknowledgedTransactions, _jso)}");

        return acknowledgedTransactions;
    }

    public async Task<Transaction?> TakeFirstAsync(TransactionOneRequest request)
    {
        _logService.Log(LogLevel.Information, $"Transaction get one request: \n{JsonSerializer.Serialize(request, _jso)}");

        var login = _configuration["Auth:Login"];

        if (string.IsNullOrEmpty(login))
        {
            _logService.Log(LogLevel.Error, "Not configured auth login");
            throw new Exception("Not configured auth login");
        }

        if (login != request.Login)
        {
            _logService.Log(LogLevel.Error, $"Incorrect login: {request.Login}");
            throw new Exception($"Incorrect login: {request.Login}");
        }

        var password = _configuration["Auth:Password"];

        if (string.IsNullOrEmpty(password))
        {
            _logService.Log(LogLevel.Error, "Not configured auth password");
            throw new Exception("Not configured auth password");
        }

        if (password != request.Password)
        {
            _logService.Log(LogLevel.Error, $"Incorrect password: {request.Password}");
            throw new Exception($"Incorrect password: {request.Password}");
        }

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

            _logService.Log(LogLevel.Information, $"Transaction get one response: \n{JsonSerializer.Serialize(transaction, _jso)}");
        }
        else
        {
            _logService.Log(LogLevel.Information, "Transaction get one not found");
        }

        return transaction;
    }
}