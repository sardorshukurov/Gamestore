using Gamestore.Domain.Entities.Northwind;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;
using MongoDB.Driver;

namespace Gamestore.DAL.Interceptors;

public class UpdateEntitiesInterceptor(IMongoDatabase database) : SaveChangesInterceptor
{
    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        var dbContext = eventData.Context;

        if (dbContext is null)
        {
            return base.SavingChangesAsync(eventData, result, cancellationToken);
        }

        var entries = dbContext.ChangeTracker.Entries();

        foreach (var entry in entries)
        {
            LogChangesAsync(entry);
        }

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    private void LogChangesAsync(EntityEntry entry)
    {
        EntityChangeLog entityChangeLog = null;

        if (entry.State == EntityState.Added)
        {
            entityChangeLog = new()
            {
                Timestamp = DateTime.UtcNow,
                Action = nameof(EntityState.Added),
                EntityType = entry.Entity.GetType().ToString(),
                NewVersion = entry.CurrentValues.Properties
                .ToDictionary(
                    prop => prop.Name,
                    prop => entry.CurrentValues[prop]?.ToString() ?? "null"),
            };
        }

        if (entry.State == EntityState.Modified)
        {
            entityChangeLog = new()
            {
                Timestamp = DateTime.UtcNow,
                Action = nameof(EntityState.Added),
                EntityType = entry.Entity.GetType().ToString(),
                OldVersion = entry.OriginalValues.Properties
                .ToDictionary(
                    prop => prop.Name,
                    prop => entry.OriginalValues[prop]?.ToString() ?? "null"),
                NewVersion = entry.CurrentValues.Properties
                .ToDictionary(
                    prop => prop.Name,
                    prop => entry.CurrentValues[prop]?.ToString() ?? "null"),
            };
        }

        if (entityChangeLog is not null)
        {
            var collection = database.GetCollection<EntityChangeLog>("entity-change-logs");
            collection.InsertOne(entityChangeLog);
        }
    }
}