using Kp.Ms.Oxi.Context;
using Kp.Ms.Oxi.Models.Entity;
using Microsoft.EntityFrameworkCore;

namespace Kp.Ms.Oxi.Services;

public class TransactionService
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<TransactionService> _logger;
    private readonly DefaultContext _context;

    public TransactionService(DefaultContext context, ILogger<TransactionService> logger, IConfiguration configuration)
    {
        _context = context;
        _logger = logger;
        _configuration = configuration;
    }

    public async Task<Transaction?> FirstOrDefault()
    {
        Transaction? transaction = await _context.Transactions
            .Where(t => t.Sended == false || (t.Sended && t.SendedAt < DateTime.Now.Subtract(TimeSpan.FromHours(1))))
            .Where(t => t.Received == false)
            .OrderBy(t=> t.Id)
            .FirstOrDefaultAsync();

        if (transaction == null) return transaction;

        transaction.Sended = true;
        transaction.SendedAt = DateTime.UtcNow;
        transaction.UpdatedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();

        return transaction;
    }
}