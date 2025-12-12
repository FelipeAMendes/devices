using Bogus;
using Devices.Domain.Devices.Entities;
using Devices.Domain.Devices.Enums;
using FluentAssertions;

namespace Devices.Domain.Tests.Devices.Entities;

[Trait("Entities", nameof(Device))]
public class DeviceTests
{
    [Fact]
    public void ShouldCreateDevice()
    {
        // Arrange
        var name = "iPhone 15";
        var brand = "Apple";
        var state = DeviceState.Available;

        // Act
        var device = new DeviceBuilder()
            .WithName(name)
            .WithBrand(brand)
            .WithState(state)
            .Build();

        // Assert
        device.Name.Should().Be(name);
        device.Brand.Should().Be(brand);
        device.State.Should().Be(state);
        device.CreationTime.Should().BeCloseTo(DateTime.Now, TimeSpan.FromSeconds(1));
    }

    [Fact]
    public void ShouldUpdateDevice()
    {
        // Arrange
        var newName = "New Name";
        var newBrand = "New Brand";
        var newState = DeviceState.Available;

        var device = new DeviceBuilder()
            .WithName("Old Name")
            .WithBrand("Old Brand")
            .WithState(DeviceState.Available)
            .Build();

        // Act
        device.Update(newName, newBrand, newState);

        // Assert
        device.Name.Should().Be(newName);
        device.Brand.Should().Be(newBrand);
        device.State.Should().Be(newState);
    }

    [Fact]
    public void ShouldAllowUpdate_WhenStateIsNotInUse()
    {
        // Arrange
        var device = new DeviceBuilder()
            .WithState(DeviceState.Available)
            .Build();

        // Act
        var result = device.CanUpdateWhenStateIsInUse("AnyName", "AnyBrand");

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void ShouldAllowUpdate_WhenStateIsInUse_AndNameAndBrandRemainSame()
    {
        // Arrange
        var device = new DeviceBuilder()
            .WithName("Laptop")
            .WithBrand("Dell")
            .WithState(DeviceState.InUse)
            .Build();

        // Act
        var result = device.CanUpdateWhenStateIsInUse("Laptop", "Dell");

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void ShouldNotAllowUpdate_WhenStateIsInUse_AndNameOrBrandChange()
    {
        // Arrange
        var device = new DeviceBuilder()
            .WithName("Laptop")
            .WithBrand("Dell")
            .WithState(DeviceState.InUse)
            .Build();

        // Act
        var result = device.CanUpdateWhenStateIsInUse("Notebook", "Dell");

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void ShouldAllowDelete_WhenDeviceIsNotInUse()
    {
        // Arrange
        var device = new DeviceBuilder()
            .WithState(DeviceState.Available)
            .Build();

        // Act
        var canDelete = device.CanDelete();

        // Assert
        canDelete.Should().BeTrue();
    }

    [Fact]
    public void ShouldNotAllowDelete_WhenDeviceIsInUse()
    {
        // Arrange
        var device = new DeviceBuilder()
            .WithState(DeviceState.InUse)
            .Build();

        // Act
        var canDelete = device.CanDelete();

        // Assert
        canDelete.Should().BeFalse();
    }

    [Fact]
    public void ShouldMarkDeviceAsDeleted()
    {
        // Arrange
        var device = new DeviceBuilder().Build();

        // Act
        var deleted = device.Delete();

        // Assert
        deleted.Should().BeTrue();
        device.Removed.Should().BeTrue();
    }

    [Fact]
    public void ShouldReturnIsInUse()
    {
        // Arrange
        var device = new DeviceBuilder()
            .WithState(DeviceState.InUse)
            .Build();

        // Act
        var result = device.IsInUse;

        // Assert
        result.Should().BeTrue();
    }
}
