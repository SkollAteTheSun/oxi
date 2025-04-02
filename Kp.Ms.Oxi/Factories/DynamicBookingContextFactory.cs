using Kp.Ms.Oxi.Context;
using Kp.Ms.Oxi.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Kp.Ms.Oxi.Factories;

public class DynamicBookingContextFactory : IDynamicBookingContextFactory
{
    private readonly IConfiguration _configuration;
    private readonly IDbContextFactory<DynamicBookingContext> _contextFactory;

    public DynamicBookingContextFactory(IDbContextFactory<DynamicBookingContext> contextFactory, IConfiguration configuration)
    {
        _contextFactory = contextFactory;
        _configuration = configuration;
    }

    public DynamicBookingContext Create(string tableName)
    {
        var context = _contextFactory.CreateDbContext();
        context.SetTableName(tableName);

        context.Database.EnsureCreated();
        return context;
    }
}