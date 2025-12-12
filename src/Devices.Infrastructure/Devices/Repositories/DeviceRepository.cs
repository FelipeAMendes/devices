using Devices.Application.Data;
using Devices.Domain.Devices.Entities;
using Devices.Domain.Devices.Repositories;
using System.Linq.Expressions;

namespace Devices.Infrastructure.Devices.Repositories;

public class DeviceRepository(IDataContext context) : IDeviceRepository
{
    private readonly IDataContext _context = context;

    public async Task<bool> ExistsAsync(Guid id, CancellationToken ct)
    {
        return await _context.AnyAsync<Device>(x => x.Id == id, ct);
    }

    public async Task<Device?> GetByIdAsync(Guid id, CancellationToken ct)
    {
        return await _context.GetByIdAsync<Device>(id, ct);
    }

    public async Task<IEnumerable<Device>> GetAllAsync(Expression<Func<Device, bool>> predicate, CancellationToken ct)
    {
        var devices = await _context
            .WhereAsync(readOnly: true, predicate, ct);

        return devices;
    }

    public async Task<bool> CreateAsync(Device device, CancellationToken ct)
    {
        return await _context.CreateAsync(device, ct);
    }

    public async Task<bool> UpdateAsync(Device device, CancellationToken ct)
    {
        return await _context.UpdateAsync(device, ct);
    }

    public async Task<bool> DeleteAsync(Device device, CancellationToken ct)
    {
        return await _context.DeleteAsync(device, ct);
    }
}
