using Devices.Application.Devices.Shared.Dtos;
using Devices.Shared.Queries;

namespace Devices.Application.Devices.GetAll.Queries;

public record GetAllDevicesResult(IEnumerable<DeviceDto> Devices) : IQueryResult;

public record GetAllDevicesQuery(string? Name, string? Brand) : IQuery<QueryResult<GetAllDevicesResult>>;
