using Bogus;
using Devices.Domain.Devices.Entities;
using Devices.Domain.Devices.Enums;

public class DeviceBuilder
{
    private string _name;
    private string _brand;
    private DeviceState _state;

    public DeviceBuilder()
    {
        var faker = new Faker();
        _name = faker.Random.String2(5);
        _brand = faker.Random.String2(5);
        _state = faker.Random.Enum<DeviceState>();
    }

    public Device Build()
    {
        return new Device(_name, _brand, _state);
    }

    public DeviceBuilder WithName(string name)
    {
        _name = name;
        return this;
    }

    public DeviceBuilder WithBrand(string brand)
    {
        _brand = brand;
        return this;
    }

    public DeviceBuilder WithState(DeviceState state)
    {
        _state = state;
        return this;
    }
}
