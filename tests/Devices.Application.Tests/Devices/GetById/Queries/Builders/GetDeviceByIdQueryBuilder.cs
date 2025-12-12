using Devices.Application.Devices.GetById.Queries;

namespace Devices.Application.Tests.Devices.GetById.Queries.Builders;

public class GetDeviceByIdQueryBuilder
{
    public GetDeviceByIdQuery Build()
    {
        var id = Guid.NewGuid();

        return new GetDeviceByIdQuery(id);
    }
}
