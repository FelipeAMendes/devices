using Devices.Application.Devices.Create.Commands;
using Devices.Application.Devices.Delete.Commands;
using Devices.Application.Devices.GetAll.Queries;
using Devices.Application.Devices.GetById.Queries;
using Devices.Application.Devices.Shared.Dtos;
using Devices.Application.Devices.Update.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Devices.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class DevicesController(ISender sender) : BaseController
{
    /// <summary>
    /// Fetch a single device.
    /// </summary>
    /// <param name="id">Identifier of the device</param>
    /// <param name="ct">cancellation token</param>
    /// <returns>device found</returns>
    [HttpGet("{id:Guid}")]
    [ProducesResponseType(typeof(DeviceDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(NotFoundResult), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetByIdAsync([FromRoute] Guid id, CancellationToken ct)
    {
        var query = new GetDeviceByIdQuery(id);

        var queryResult = await sender.Send(query, ct);

        return GenerateResponse(queryResult, x => x.Result);
    }

    /// <summary>
    /// Fetch all devices or by name/brand.
    /// </summary>
    /// <param name="name">device name</param>
    /// <param name="brand">device brand</param>
    /// <param name="ct">cancellation token</param>
    /// <returns>device found</returns>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<DeviceDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAsync([FromQuery] string? name, string? brand, CancellationToken ct)
    {
        var query = new GetAllDevicesQuery(name, brand);

        var queryResult = await sender.Send(query, ct);

        return GenerateResponse(queryResult, x => x.Result);
    }

    /// <summary>
    /// Create a new device.
    /// </summary>
    /// <param name="command">input data</param>
    /// <param name="ct">cancellation token</param>
    /// <returns>success or error</returns>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(BadRequestResult), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(NotFoundResult), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> PostAsync([FromBody] CreateDeviceCommand command, CancellationToken ct)
    {
        var commandResult = await sender.Send(command, ct);

        return this.ToActionResult(commandResult);
    }

    /// <summary>
    /// Fully and/or partially update an exis:ng device.
    /// </summary>
    /// <param name="command">input data</param>
    /// <param name="ct">cancellation token</param>
    /// <returns>success or error</returns>
    [HttpPut]
    [ProducesResponseType(typeof(NoContentResult), StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(BadRequestResult), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(NotFoundResult), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> PutAsync([FromBody] UpdateDeviceCommand command, CancellationToken ct)
    {
        var commandResult = await sender.Send(command, ct);

        return this.ToActionResult(commandResult);
    }

    /// <summary>
    /// Delete a single device.
    /// </summary>
    /// <param name="id">Identifier of the device</param>
    /// <param name="ct">cancellation token</param>
    /// <returns>success or error</returns>
    [HttpDelete("{id:Guid}")]
    [ProducesResponseType(typeof(NoContentResult), StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(BadRequestResult), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(NotFoundResult), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteAsync([FromRoute] Guid id, CancellationToken ct)
    {
        var command = new DeleteDeviceCommand(id);

        var commandResult = await sender.Send(command, ct);

        return this.ToActionResult(commandResult);
    }
}
