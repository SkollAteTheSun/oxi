using Kp.Ms.Oxi.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Kp.Ms.Oxi.Interfaces;

public interface IDynamicBookingContextFactory
{
    DynamicBookingContext Create(string tableName);
}