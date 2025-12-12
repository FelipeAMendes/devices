using Devices.Application.Devices.Shared.Dtos;
using Devices.Domain.Devices.Entities;
using Devices.Domain.Devices.Repositories;
using Devices.Shared.Extensions.ExpressionExtensions;
using Devices.Shared.Extensions.StringExtensions;
using Devices.Shared.Queries;
using Mapster;
using System.Linq.Expressions;

namespace Devices.Application.Devices.GetAll.Queries;

public class GetAllDevicesHandler(IDeviceRepository resumeRepository): IQueryHandler<GetAllDevicesQuery, QueryResult<GetAllDevicesResult>>
{
    private readonly IDeviceRepository _resumeRepository = resumeRepository;

    public async Task<QueryResult<GetAllDevicesResult>> Handle(GetAllDevicesQuery command, CancellationToken ct)
    {
        Expression<Func<Device, bool>> filter = PredicateBuilder.True<Device>();

        if (command.Name!.HasValue())
            filter = filter.And(x => x.Name.Contains(command.Name!));

        if (command.Brand!.HasValue())
            filter = filter.And(x => x.Brand.Contains(command.Brand!));

        var devices = await _resumeRepository.GetAllAsync(filter, ct);

        var devicesDto = devices.Adapt<IEnumerable<DeviceDto>>();

        var result = new GetAllDevicesResult(devicesDto);
        return QueryResult<GetAllDevicesResult>.Ok(result);
    }
}
