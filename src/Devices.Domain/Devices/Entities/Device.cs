using Devices.Domain.Devices.Enums;
using Devices.Domain.Devices.Specifications;
using Devices.Shared.Entities;
using FluentValidation;

namespace Devices.Domain.Devices.Entities;

public class Device : Entity<DeviceValidator>
{
    public string Name { get; private set; }
    public string Brand { get; private set; }
    public DeviceState State { get; private set; }

    public Device(string name, string brand, DeviceState state)
    {
        Name = name;
        Brand = brand;
        State = state;
        CreationTime = DateTime.Now;
    }

    public void Update(string name, string brand, DeviceState state)
    {
        Name = name;
        Brand = brand;
        State = state;
    }
}

public class DeviceValidator : AbstractValidator<Device>
{
	public DeviceValidator()
	{
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(DeviceSpecification.NameColumnSize);

        RuleFor(x => x.Brand)
            .NotEmpty()
            .MaximumLength(DeviceSpecification.BrandColumnSize);

        RuleFor(x => x.State)
            .IsInEnum();
    }
}