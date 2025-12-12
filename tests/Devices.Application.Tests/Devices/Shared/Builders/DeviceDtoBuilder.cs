using Bogus;
using Devices.Application.Devices.Shared.Dtos;
using Devices.Domain.Devices.Enums;

namespace Devices.Application.Tests.Devices.Shared.Builders;

public class DeviceDtoBuilder
{
    private readonly Faker faker;
    private string _name;
    private string _brand;
    private DeviceState _state;

    public DeviceDtoBuilder()
    {
        faker = new Faker();
        _name = faker.Random.String2(5);
        _brand = faker.Random.String2(5);
        _state = faker.Random.Enum<DeviceState>();
    }

    public DeviceDto Build()
    {
        var id = faker.Random.Guid();
        var creationTime = faker.Date.Recent();

        return new DeviceDto(id, _name, _brand, _state, creationTime);
    }

    public DeviceDtoBuilder WithName(string name)
    {
        _name = name;
        return this;
    }

    public DeviceDtoBuilder WithBrand(string brand)
    {
        _brand = brand;
        return this;
    }

    public DeviceDtoBuilder WithState(DeviceState state)
    {
        _state = state;
        return this;
    }
}
