using Bogus;
using Devices.Domain.Devices.Entities;
using Devices.Domain.Devices.Enums;

namespace Devices.Application.Tests.Shared.Builders;

public class DeviceBuilder
{
    private readonly Faker faker;
    private string name;
    private DeviceState state;

    public DeviceBuilder()
    {
        faker = new Faker();
        name = faker.Random.String2(5);
        state = faker.Random.Enum<DeviceState>();
    }

    public Device Build()
    {
        var brand = faker.Random.String2(5);

        return new Device(name, brand, state);
    }

    public DeviceBuilder WithName(string name)
    {
        this.name = name;
        return this;
    }

    public DeviceBuilder WithState(DeviceState state)
    {
        this.state = state;
        return this;
    }
}
