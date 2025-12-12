using Devices.Application.Devices.Shared.Dtos;
using Devices.Domain.Devices.Repositories;
using Devices.Shared.Queries;
using Mapster;

namespace Devices.Application.Devices.GetById.Queries;

public class GetDeviceByIdHandler(IDeviceRepository resumeRepository): IQueryHandler<GetDeviceByIdQuery, QueryResult<GetDeviceByIdResult>>
{
    private readonly IDeviceRepository _resumeRepository = resumeRepository;

    public async Task<QueryResult<GetDeviceByIdResult>> Handle(GetDeviceByIdQuery command, CancellationToken ct)
    {
        if (await _resumeRepository.GetByIdAsync(command.Id, ct) is var existingDevice && existingDevice is null)
            return QueryResult<GetDeviceByIdResult>.NotFound();

        var device = existingDevice.Adapt<DeviceDto>();

        var result = new GetDeviceByIdResult(device);
        return QueryResult<GetDeviceByIdResult>.Ok(result);
    }
}
