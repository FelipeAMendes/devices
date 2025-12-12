using Devices.Domain.Devices.Entities;
using System.Linq.Expressions;

namespace Devices.Domain.Devices.Repositories;

public interface IDeviceRepository
{
    Task<bool> ExistsAsync(Guid id, CancellationToken ct);
    Task<Device?> GetByIdAsync(Guid id, CancellationToken ct);
    Task<Device?> GetByExpressionAsync(Expression<Func<Device, bool>> predicate, CancellationToken ct);
    Task<IEnumerable<Device>> GetAllAsync(CancellationToken ct);
    Task<bool> CreateAsync(Device device, CancellationToken ct);
    Task<bool> UpdateAsync(Device device, CancellationToken ct);
    Task<bool> DeleteAsync(Device device, CancellationToken ct);
}
