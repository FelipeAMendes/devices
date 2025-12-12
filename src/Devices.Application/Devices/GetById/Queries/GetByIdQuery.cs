using Devices.Application.Devices.Shared.Dtos;
using Devices.Shared.Queries;

namespace Devices.Application.Devices.GetById.Queries;

public record GetDeviceByIdResult(DeviceDto Device) : IQueryResult;

public record GetDeviceByIdQuery(Guid Id) : IQuery<QueryResult<GetDeviceByIdResult>>;
