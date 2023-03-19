using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace poc_shared_context.domain
{
    public interface IAuditable
    {

        public DateTime CreatedOn { get; set; }
        public DateTime DeletedOn { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime UpdatedOn { get; set; }
    }

    public abstract class ReadOnlyDbContext : DbContext
    {
        public override EntityEntry<TEntity> Add<TEntity>(TEntity entity)
        {
            throw new InvalidOperationException("This context is read-only.");
        }

        public override EntityEntry Add(object entity)
        {
            throw new InvalidOperationException("This context is read-only.");
        }

        public override ValueTask<EntityEntry<TEntity>> AddAsync<TEntity>(TEntity entity, CancellationToken cancellationToken = default)
        {
            throw new InvalidOperationException("This context is read-only.");
        }

        public override ValueTask<EntityEntry> AddAsync(object entity, CancellationToken cancellationToken = default)
        {
            throw new InvalidOperationException("This context is read-only.");
        }

        public override void AddRange(params object[] entities)
        {
            throw new InvalidOperationException("This context is read-only.");
        }

        public override void AddRange(IEnumerable<object> entities)
        {
            throw new InvalidOperationException("This context is read-only.");
        }

        public override Task AddRangeAsync(params object[] entities)
        {
            throw new InvalidOperationException("This context is read-only.");
        }

        public override Task AddRangeAsync(IEnumerable<object> entities, CancellationToken cancellationToken = default)
        {
            throw new InvalidOperationException("This context is read-only.");
        }

        public override EntityEntry<TEntity> Attach<TEntity>(TEntity entity)
        {
            throw new InvalidOperationException("This context is read-only.");
        }

        public override EntityEntry Attach(object entity)
        {
            throw new InvalidOperationException("This context is read-only.");
        }

        public override void AttachRange(params object[] entities)
        {
            throw new InvalidOperationException("This context is read-only.");
        }

        public override void AttachRange(IEnumerable<object> entities)
        {
            throw new InvalidOperationException("This context is read-only.");
        }

        public override EntityEntry<TEntity> Remove<TEntity>(TEntity entity)
        {
            throw new InvalidOperationException("This context is read-only.");
        }

        public override EntityEntry Remove(object entity)
        {
            throw new InvalidOperationException("This context is read-only.");
        }

        public override void RemoveRange(params object[] entities)
        {
            throw new InvalidOperationException("This context is read-only.");
        }

        public override void RemoveRange(IEnumerable<object> entities)
        {
            throw new InvalidOperationException("This context is read-only.");
        }

        public override int SaveChanges()
        {
            throw new InvalidOperationException("This context is read-only.");
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            throw new InvalidOperationException("This context is read-only.");
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            throw new InvalidOperationException("This context is read-only.");
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            throw new InvalidOperationException("This context is read-only.");
        }

        public override EntityEntry<TEntity> Update<TEntity>(TEntity entity)
        {
            throw new InvalidOperationException("This context is read-only.");
        }

        public override EntityEntry Update(object entity)
        {
            throw new InvalidOperationException("This context is read-only.");
        }

        public override void UpdateRange(params object[] entities)
        {
            throw new InvalidOperationException("This context is read-only.");
        }

        public override void UpdateRange(IEnumerable<object> entities)
        {
            throw new InvalidOperationException("This context is read-only.");
        }
    }
}