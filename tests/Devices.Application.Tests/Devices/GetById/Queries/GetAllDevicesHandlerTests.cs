using Devices.Application.Devices.GetById.Queries;
using Devices.Application.Tests.Devices.GetById.Queries.Builders;
using Devices.Application.Tests.Shared;
using Devices.Application.Tests.Shared.Builders;
using Devices.Domain.Devices.Repositories;
using Devices.Shared.Responses;
using FluentAssertions;
using NSubstitute;
using NSubstitute.ReturnsExtensions;

namespace Devices.Application.Tests.Devices.GetById.Queries;

[Trait("Queries", nameof(GetDeviceByIdHandler))]
public class GetDeviceByIdHandlerTests
{
    private readonly IDeviceRepository _repository;
    private readonly CancellationToken _ct;
    private readonly GetDeviceByIdHandler _handler;

    public GetDeviceByIdHandlerTests()
    {
        _repository = Substitute.For<IDeviceRepository>();
        _handler = new GetDeviceByIdHandler(_repository);

        _ct = new CancellationTokenBuilder()
            .Build();

        MapsterUnitTests.ConfigurarMappers();
    }

    [Fact]
    public async Task ShouldReturnDeviceById()
    {
        // Arrange
        var query = new GetDeviceByIdQueryBuilder()
            .Build();

        var device = new DeviceBuilder()
            .Build();

        _repository
            .GetByIdAsync(query.Id, _ct)
            .Returns(device);

        // Act
        var result = await _handler.Handle(query, _ct);

        // Assert
        result.Status.Should().Be(ResponseStatus.Ok);
        result.Result!.Device.Should().NotBeNull();

        await _repository
            .ReceivedWithAnyArgs(1)
            .GetByIdAsync(query.Id, _ct);
    }

    [Fact]
    public async Task ShouldReturnNotFound_WhenDeviceDoesNotExist()
    {
        // Arrange
        var query = new GetDeviceByIdQueryBuilder()
            .Build();

        _repository
            .GetByIdAsync(query.Id, _ct)
            .ReturnsNull();

        // Act
        var result = await _handler.Handle(query, _ct);

        // Assert
        result.Status.Should().Be(ResponseStatus.NotFound);
        result.Result?.Device?.Should().BeNull();

        await _repository
            .ReceivedWithAnyArgs(1)
            .GetByIdAsync(query.Id, _ct);
    }
}
