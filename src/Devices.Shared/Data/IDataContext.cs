using Devices.Shared.Entities;
using System.Data;
using System.Linq.Expressions;

namespace Devices.Application.Data;

public interface IDataContext
{
    Task<bool> CreateAsync<TEntity>(TEntity entity, CancellationToken ct) where TEntity : IEntity;
    Task<bool> UpdateAsync<TEntity>(TEntity entity, CancellationToken ct) where TEntity : IEntity;
    Task<bool> DeleteAsync<TEntity>(TEntity entity, CancellationToken ct) where TEntity : IEntity;
    Task<TEntity?> GetByIdAsync<TEntity>(Guid id, CancellationToken ct) where TEntity : class, IEntity;
    Task<List<TEntity>> WhereAsync<TEntity>(bool readOnly, Expression<Func<TEntity, bool>> where, CancellationToken ct) where TEntity : class;
    Task<bool> AnyAsync<TEntity>(Expression<Func<TEntity, bool>> where, CancellationToken ct) where TEntity : class;

    Task BeginTransactionAsync(IsolationLevel isolationLevel = IsolationLevel.Serializable);
    Task CommitAsync();
    Task RollbackAsync();
}