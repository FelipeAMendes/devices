using Devices.Application.Devices.Update.Commands;
using Devices.Application.Tests.Devices.Shared.Builders;
using Devices.Application.Tests.Devices.Update.Commands.Builders;
using Devices.Application.Tests.Shared;
using Devices.Application.Tests.Shared.Builders;
using Devices.Domain.Devices.Enums;
using Devices.Domain.Devices.Repositories;
using Devices.Shared.Responses;
using FluentAssertions;
using NSubstitute;
using NSubstitute.ReturnsExtensions;

namespace Devices.Application.Tests.Devices.Update.Commands;

[Trait("Handlers", nameof(UpdateDeviceCommand))]
public class UpdateDeviceHandlerTests
{
    private readonly IDeviceRepository _repository;
    private readonly CancellationToken _ct;
    private readonly UpdateDeviceHandler _handler;

    public UpdateDeviceHandlerTests()
    {
        _repository = Substitute.For<IDeviceRepository>();
        _handler = new UpdateDeviceHandler(_repository);

        _ct = new CancellationTokenBuilder()
            .Build();

        MapsterUnitTests.ConfigurarMappers();
    }

    [Fact]
    public async Task ShouldUpdateDevice()
    {
        // Arrange
        var deviceDto = new DeviceDtoBuilder()
            .WithState(DeviceState.Available)
            .Build();

        var command = new UpdateDeviceCommandBuilder()
            .WithDeviceDto(deviceDto)
            .Build();

        var device = new DeviceBuilder()
            .WithState(DeviceState.Available)
            .Build();

        _repository
            .GetByIdAsync(command.Device.Id, _ct)
            .Returns(device);

        _repository
            .UpdateAsync(default!, _ct)
            .ReturnsForAnyArgs(true);

        // Act
        var result = await _handler.Handle(command, _ct);

        // Assert
        result.Status.Should().Be(ResponseStatus.NoContent);

        await _repository
            .Received(1)
            .GetByIdAsync(command.Device.Id, _ct);

        await _repository
            .ReceivedWithAnyArgs(1)
            .UpdateAsync(default!, _ct);
    }

    [Fact]
    public async Task ShouldReturnNotFound_WhenDeviceDoesNotExist()
    {
        // Arrange
        var command = new UpdateDeviceCommandBuilder()
            .Build();

        _repository
            .GetByIdAsync(command.Device.Id, _ct)
            .ReturnsNull();

        // Act
        var result = await _handler.Handle(command, _ct);

        // Assert
        result.Status.Should().Be(ResponseStatus.NotFound);

        await _repository
            .Received(1)
            .GetByIdAsync(command.Device.Id, _ct);

        await _repository
            .DidNotReceiveWithAnyArgs()
            .UpdateAsync(default!, _ct);
    }

    [Fact]
    public async Task ShouldReturnBadRequest_WhenDeviceIsInUseAndNameOrBrandCannotBeUpdated()
    {
        // Arrange
        var command = new UpdateDeviceCommandBuilder()
            .Build();

        var device = new DeviceBuilder()
            .WithState(DeviceState.InUse)
            .Build();

        _repository
            .GetByIdAsync(command.Device.Id, _ct)
            .Returns(device);

        // Act
        var result = await _handler.Handle(command, _ct);

        // Assert
        result.Status.Should().Be(ResponseStatus.BadRequest);
        result.Errors.First()!.ErrorMessage.Should().Be("Name and brand cannot be updated if state is in use.");

        await _repository
            .Received(1)
            .GetByIdAsync(command.Device.Id, _ct);

        await _repository
            .DidNotReceiveWithAnyArgs()
            .UpdateAsync(default!, _ct);
    }

    [Fact]
    public async Task ShouldReturnBadRequest_WhenUpdatedDeviceIsInvalid()
    {
        // Arrange
        var deviceDto = new DeviceDtoBuilder()
            .WithName(default!)
            .Build();

        var command = new UpdateDeviceCommandBuilder()
            .WithDeviceDto(deviceDto)
            .Build();

        var device = new DeviceBuilder()
            .Build();

        _repository
            .GetByIdAsync(command.Device.Id, _ct)
            .Returns(device);

        // Act
        var result = await _handler.Handle(command, _ct);

        // Assert
        result.Status.Should().Be(ResponseStatus.BadRequest);

        await _repository
            .Received(1)
            .GetByIdAsync(command.Device.Id, _ct);

        await _repository
            .DidNotReceiveWithAnyArgs()
            .UpdateAsync(default!, _ct);
    }

    [Fact]
    public async Task ShouldReturnBadRequest_WhenRepositoryUpdateFails()
    {
        // Arrange
        var command = new UpdateDeviceCommandBuilder()
            .Build();

        var device = new DeviceBuilder()
            .Build();

        _repository
            .GetByIdAsync(command.Device.Id, _ct)
            .Returns(device);

        _repository
            .UpdateAsync(default!, _ct)
            .ReturnsForAnyArgs(false);

        // Act
        var result = await _handler.Handle(command, _ct);

        // Assert
        result.Status.Should().Be(ResponseStatus.BadRequest);
        result.Errors.First()!.ErrorMessage.Should().Be("An error has ocurred when trying to update the device.");

        await _repository
            .Received(1)
            .GetByIdAsync(command.Device.Id, _ct);

        await _repository
            .ReceivedWithAnyArgs(1)
            .UpdateAsync(default!, _ct);
    }
}
