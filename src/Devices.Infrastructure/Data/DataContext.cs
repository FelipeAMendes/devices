using Devices.Application.Data;
using Devices.Infrastructure.Data.Extensions;
using Devices.Shared.Entities;
using Devices.Shared.Extensions.IQueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Data;
using System.Linq.Expressions;

namespace Devices.Infrastructure.Data;

public class DataContext(AppDbContext applicationContext) : IDataContext
{
    public const int MaxBatchSize = 50;
    private readonly AppDbContext _applicationContext = applicationContext;
    private IDbContextTransaction? _transaction;

    public async Task<bool> CreateAsync<TEntity>(TEntity entity, CancellationToken ct) where TEntity : IEntity
    {
        async Task<bool> Insert()
        {
            _applicationContext.Add(entity);

            var result = await _applicationContext.SaveChangesAsync(ct);
            return result > 0;
        }

        return await this.ExecuteStrategyAndRetry(_applicationContext, Insert);
    }

    public async Task<bool> UpdateAsync<TEntity>(TEntity entity, CancellationToken ct) where TEntity : IEntity
    {
        async Task<bool> Update()
        {
            _applicationContext.Update(entity);

            var result = await _applicationContext.SaveChangesAsync(ct);
            return result > 0;
        }

        return await this.ExecuteStrategyAndRetry(_applicationContext, Update);
    }

    public async Task<bool> DeleteAsync<TEntity>(TEntity entity, CancellationToken ct) where TEntity : IEntity
    {
        async Task<bool> Delete()
        {
            _applicationContext.Remove(entity);

            var result = await _applicationContext.SaveChangesAsync(ct);
            return result > 0;
        }

        return await this.ExecuteStrategyAndRetry(_applicationContext, Delete);
    }

    public async Task<TEntity?> GetByIdAsync<TEntity>(Guid id, CancellationToken ct) where TEntity : class, IEntity
    {
        async Task<TEntity?> GetById()
        {
            var entity = await _applicationContext
                .GetDbSet<TEntity>()
                .FindAsync([id], cancellationToken: ct);

            return entity;
        }

        return await this.ExecuteRetry(GetById);
    }

    public async Task<TEntity?> FirstOrDefaultAsync<TEntity>(bool readOnly, Expression<Func<TEntity, bool>> where, CancellationToken ct)
        where TEntity : class
    {
        async Task<TEntity?> GetFirstOrDefault()
        {
            var entity = await _applicationContext.GetDbSet<TEntity>()
                .AsNoTracking(readOnly)
                .FirstOrDefaultAsync(where, ct);

            return entity;
        }

        return await this.ExecuteRetry(GetFirstOrDefault);
    }

    public async Task<List<TEntity>> AllAsync<TEntity>(CancellationToken ct) where TEntity : class
    {
        async Task<List<TEntity>> GetAll()
        {
            var entities = await _applicationContext
                .GetDbSet<TEntity>()
                .AsNoTracking()
                .ToListAsync(ct);

            return entities;
        }

        return await this.ExecuteRetry(GetAll);
    }

    public async Task<bool> AnyAsync<TEntity>(Expression<Func<TEntity, bool>> where, CancellationToken ct) where TEntity : class
    {
        async Task<bool> GetAny()
        {
            var any = await _applicationContext
                .GetDbSet<TEntity>()
                .AsNoTracking()
                .AnyAsync(where, ct);

            return any;
        }

        return await this.ExecuteRetry(GetAny);
    }

    public async Task BeginTransactionAsync(IsolationLevel isolationLevel = IsolationLevel.Serializable)
    {
        await this.ExecuteStrategyAndRetry(_applicationContext,
            async () =>
            {
                _transaction = await _applicationContext.Database.BeginTransactionAsync(isolationLevel);
            });
    }

    public async Task CommitAsync()
    {
        await this.ExecuteStrategyAndRetry(_applicationContext,
            async () =>
            {
                if (_transaction is not null)
                    await _transaction.CommitAsync();
            });
    }

    public async Task RollbackAsync()
    {
        await this.ExecuteStrategyAndRetry(_applicationContext,
            async () =>
            {
                if (_transaction is not null)
                    await _transaction.RollbackAsync();
            });
    }
}