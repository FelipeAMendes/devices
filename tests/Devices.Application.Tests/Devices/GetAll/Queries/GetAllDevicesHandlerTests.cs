using Devices.Application.Devices.GetAll.Queries;
using Devices.Application.Tests.Devices.GetAll.Queries.Builders;
using Devices.Application.Tests.Shared;
using Devices.Application.Tests.Shared.Builders;
using Devices.Domain.Devices.Entities;
using Devices.Domain.Devices.Repositories;
using Devices.Shared.Responses;
using FluentAssertions;
using NSubstitute;

namespace Devices.Application.Tests.Devices.GetAll.Queries;

[Trait("Queries", nameof(GetAllDevicesHandler))]
public class GetAllDevicesHandlerTests
{
    private readonly IDeviceRepository _repository;
    private readonly CancellationToken _ct;
    private readonly GetAllDevicesHandler _handler;

    public GetAllDevicesHandlerTests()
    {
        _repository = Substitute.For<IDeviceRepository>();
        _handler = new GetAllDevicesHandler(_repository);

        _ct = new CancellationTokenBuilder()
            .Build();

        MapsterUnitTests.ConfigurarMappers();
    }

    [Fact]
    public async Task ShouldReturnAllDevices()
    {
        // Arrange
        IEnumerable<Device> devices =
        [
            new DeviceBuilder().Build(),
            new DeviceBuilder().Build()
        ];

        _repository
            .GetAllAsync(default!, _ct)
            .ReturnsForAnyArgs(devices);

        var query = new GetAllDevicesQueryBuilder()
            .Build();

        // Act
        var result = await _handler.Handle(query, _ct);

        // Assert
        result.Status.Should().Be(ResponseStatus.Ok);
        result.Result!.Devices.Should().HaveCount(2);

        await _repository
            .ReceivedWithAnyArgs(1)
            .GetAllAsync(default!, _ct);
    }
}
