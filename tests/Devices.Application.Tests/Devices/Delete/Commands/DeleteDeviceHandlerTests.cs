using Devices.Application.Devices.Delete.Commands;
using Devices.Application.Tests.Devices.Delete.Commands.Builders;
using Devices.Application.Tests.Shared;
using Devices.Application.Tests.Shared.Builders;
using Devices.Domain.Devices.Enums;
using Devices.Domain.Devices.Repositories;
using Devices.Shared.Responses;
using FluentAssertions;
using NSubstitute;
using NSubstitute.ReturnsExtensions;

namespace Devices.Application.Tests.Devices.Delete.Commands;

[Trait("Handlers", nameof(DeleteDeviceCommand))]
public class DeleteDeviceHandlerTests
{
    private readonly IDeviceRepository _repository;
    private readonly CancellationToken _ct;
    private readonly DeleteDeviceHandler _handler;

    public DeleteDeviceHandlerTests()
    {
        _repository = Substitute.For<IDeviceRepository>();
        _handler = new DeleteDeviceHandler(_repository);

        _ct = new CancellationTokenBuilder()
            .Build();

        MapsterUnitTests.ConfigurarMappers();
    }

    [Fact]
    public async Task ShouldDeleteDevice()
    {
        // Arrange
        var command = new DeleteDeviceCommandBuilder()
            .Build();

        var device = new DeviceBuilder()
            .WithState(DeviceState.Available)
            .Build();

        _repository
            .GetByIdAsync(command.Id, _ct)
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
            .GetByIdAsync(command.Id, _ct);

        await _repository
            .ReceivedWithAnyArgs(1)
            .UpdateAsync(default!, _ct);
    }

    [Fact]
    public async Task ShouldReturnNotFound_WhenDeviceDoesNotExist()
    {
        // Arrange
        var command = new DeleteDeviceCommandBuilder()
            .Build();

        _repository
            .GetByIdAsync(command.Id, _ct)
            .ReturnsNull();

        _repository
            .UpdateAsync(default!, _ct)
            .ReturnsForAnyArgs(true);

        // Act
        var result = await _handler.Handle(command, _ct);

        // Assert
        result.Status.Should().Be(ResponseStatus.NotFound);

        await _repository
            .Received(1)
            .GetByIdAsync(command.Id, _ct);

        await _repository
            .DidNotReceiveWithAnyArgs()
            .UpdateAsync(default!, _ct);
    }

    [Fact]
    public async Task ShouldReturnBadRequest_WhenDeviceCannotBeDeleted()
    {
        // Arrange
        var command = new DeleteDeviceCommandBuilder()
            .Build();

        var device = new DeviceBuilder()
            .WithState(DeviceState.InUse)
            .Build();

        _repository
            .GetByIdAsync(command.Id, _ct)
            .Returns(device);

        _repository
            .UpdateAsync(default!, _ct)
            .ReturnsForAnyArgs(true);

        // Act
        var result = await _handler.Handle(command, _ct);

        // Assert
        result.Status.Should().Be(ResponseStatus.BadRequest);
        result.Errors.First()!.ErrorMessage.Should().Be("In use devices cannot be deleted.");

        await _repository
            .Received(1)
            .GetByIdAsync(command.Id, _ct);

        await _repository
            .DidNotReceiveWithAnyArgs()
            .UpdateAsync(default!, _ct);
    }

    [Fact]
    public async Task ShouldReturnBadRequest_WhenRepositoryUpdateFailsWhileDeletingDevice()
    {
        // Arrange
        var command = new DeleteDeviceCommandBuilder()
            .Build();

        var device = new DeviceBuilder()
            .WithState(DeviceState.Available)
            .Build();

        _repository
            .GetByIdAsync(command.Id, _ct)
            .Returns(device);

        _repository
            .UpdateAsync(default!, _ct)
            .ReturnsForAnyArgs(false);

        // Act
        var result = await _handler.Handle(command, _ct);

        // Assert
        result.Status.Should().Be(ResponseStatus.BadRequest);
        result.Errors.First()!.ErrorMessage.Should().Be("An error has ocurred when trying to delete the device.");

        await _repository
            .Received(1)
            .GetByIdAsync(command.Id, _ct);

        await _repository
            .ReceivedWithAnyArgs(1)
            .UpdateAsync(default!, _ct);
    }
}
