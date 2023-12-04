using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Readerover.Domain.Common.Models;

namespace Readerover.Persistence.Interceptors;

public class UpdateAuditableInterceptor : SaveChangesInterceptor
{
    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
    {
        if (eventData.Context is not null)
            UpdateAuditableEntities(eventData.Context);

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    private void UpdateAuditableEntities(DbContext context)
    {
        var entityEntries = context.ChangeTracker.Entries<IAuditableEntity>().ToList();

        foreach(var entry in entityEntries)
        {
            if(entry.State == EntityState.Added)
            {
                entry.Property(nameof(AuditableEntity.Id)).CurrentValue = Guid.NewGuid();
                entry.Property(nameof(AuditableEntity.CreatedDate)).CurrentValue = DateTime.UtcNow;
            }

            if(entry.State == EntityState.Deleted)
            {
                entry.Property(nameof(AuditableEntity.DeletedDate)).CurrentValue = DateTime.UtcNow;
                entry.Property(nameof(AuditableEntity.IsDeleted)).CurrentValue = true;
                entry.State = EntityState.Modified;
            }

            if(entry.State == EntityState.Modified)
            {
                entry.Property(nameof(AuditableEntity.ModifiedDate)).CurrentValue = DateTime.UtcNow;
            }
        }
    }
}
