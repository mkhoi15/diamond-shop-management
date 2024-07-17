using BusinessObject.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace DataAccessLayer.Interceptor;

public class UpdateUserInterceptor : SaveChangesInterceptor
{
    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result,
        CancellationToken cancellationToken = new CancellationToken())
    {
        DbContext? context = eventData.Context;
        
        if (context == null)
        {
            return base.SavingChangesAsync(eventData, result, cancellationToken);
        }
        
        var entities = context.ChangeTracker
            .Entries<User>()
            .Where(e => e.State == EntityState.Modified)
            .Select(e => e.Entity)
            .ToList();

        foreach (var entity in entities)
        {
            entity.UpdatedAt = DateTime.UtcNow;
        }
        
        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }
}