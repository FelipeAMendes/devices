using Devices.Application.Devices.Create.Commands;
using Devices.Application.Tests.Devices.Create.Commands.Builders;
using Devices.Application.Tests.Devices.Shared.Builders;
using Devices.Application.Tests.Shared;
using Devices.Application.Tests.Shared.Builders;
using Devices.Domain.Devices.Repositories;
using Devices.Shared.Responses;
using FluentAssertions;
using NSubstitute;

namespace Devices.Application.Tests.Devices.Create.Commands;

[Trait("Handlers", nameof(CreateDeviceCommand))]
public class CreateDeviceHandlerTests
{
    private readonly IDeviceRepository _repository;
    private readonly CancellationToken _ct;
    private readonly CreateDeviceHandler _handler;

    public CreateDeviceHandlerTests()
    {
        _repository = Substitute.For<IDeviceRepository>();
        _handler = new CreateDeviceHandler(_repository);

        _ct = new CancellationTokenBuilder()
            .Build();

        MapsterUnitTests.ConfigurarMappers();
    }

    [Fact]
    public async Task ShouldCreateDevice()
    {
        // Arrange
        var command = new CreateDeviceCommandBuilder()
            .Build();

        _repository
            .CreateAsync(default!, _ct)
            .ReturnsForAnyArgs(true);

        // Act
        var result = await _handler.Handle(command, _ct);

        // Assert
        result.Status.Should().Be(ResponseStatus.Created);

        await _repository
            .ReceivedWithAnyArgs(1)
            .CreateAsync(default!, _ct);
    }

    [Fact]
    public async Task ShouldReturnBadRequest_WhenDeviceValidationFails()
    {
        // Arrange
        var invalidDto = new DeviceDtoBuilder()
            .WithName(default!)
            .Build();

        var command = new CreateDeviceCommandBuilder()
            .WithDeviceDto(invalidDto)
            .Build();

        // Act
        var result = await _handler.Handle(command, _ct);

        // Assert
        result.Status.Should().Be(ResponseStatus.BadRequest);

        await _repository
            .DidNotReceiveWithAnyArgs()
            .CreateAsync(default!, _ct);
    }

    [Fact]
    public async Task ShouldReturnBadRequest_WhenRepositoryCreateFails()
    {
        // Arrange
        var command = new CreateDeviceCommandBuilder()
            .Build();

        _repository
            .CreateAsync(default!, _ct)
            .ReturnsForAnyArgs(false);

        // Act
        var result = await _handler.Handle(command, _ct);

        // Assert
        result.Status.Should().Be(ResponseStatus.BadRequest);

        await _repository
            .ReceivedWithAnyArgs(1)
            .CreateAsync(default!, _ct);
    }
}
