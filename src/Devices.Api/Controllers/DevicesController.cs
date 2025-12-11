using Microsoft.AspNetCore.Mvc;

namespace Devices.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class DevicesController : ControllerBase
{
    private readonly ILogger<DevicesController> _logger;

    public DevicesController(ILogger<DevicesController> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    public IActionResult Get()
    {
        return Ok();
    }
}
