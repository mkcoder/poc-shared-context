using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using poc_shared_context.domain;

namespace poc_shared_context.logic;

public static class ChangeTrackerExtensions
{
    public static void SetAuditProperties(this ChangeTracker changeTracker)
    {
        changeTracker.DetectChanges();
        IEnumerable<EntityEntry> entities =
            changeTracker
                .Entries()
                .Where(t => t.Entity is IAuditable &&
                (
                    t.State == EntityState.Deleted
                    || t.State == EntityState.Added
                    || t.State == EntityState.Modified
                ));

        if (entities.Any())
        {
            DateTime timestamp = DateTime.UtcNow;

            foreach (EntityEntry entry in entities)
            {
                IAuditable entity = (IAuditable)entry.Entity;

                switch (entry.State)
                {
                    case EntityState.Added:
                        entity.CreatedOn = timestamp;
                        entity.UpdatedOn = timestamp;
                        break;
                    case EntityState.Modified:
                        entity.UpdatedOn = timestamp;
                        break;
                    case EntityState.Deleted:
                        entity.DeletedOn = timestamp;
                        entity.UpdatedOn = timestamp;
                        entity.IsDeleted = true;
                        entry.State = EntityState.Modified;
                        break;
                }
            }
        }
    }
}