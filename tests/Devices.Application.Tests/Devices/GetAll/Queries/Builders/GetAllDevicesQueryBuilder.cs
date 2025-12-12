using Bogus;
using Devices.Application.Devices.GetAll.Queries;

namespace Devices.Application.Tests.Devices.GetAll.Queries.Builders;

public class GetAllDevicesQueryBuilder
{
    public GetAllDevicesQuery Build()
    {
        var faker = new Faker();
        var name = faker.Random.String2(5);
        var brand = faker.Random.String2(5);

        return new GetAllDevicesQuery(name, brand);
    }
}
