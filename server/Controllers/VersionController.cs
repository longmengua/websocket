using Microsoft.AspNetCore.Mvc;
using System.Reflection;

namespace server.Controllers;

[ApiController]
[Route("/")]
public class VersionController : ControllerBase
{
    private readonly ILogger<WeatherForecastController> _logger;

    public VersionController(ILogger<WeatherForecastController> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// 取得server 版本號碼
    /// </summary>
    /// <returns></returns>
    [HttpGet("")]
    public IActionResult GetServerVersion()
    {
        try
        {
            var version = Assembly.GetExecutingAssembly().GetName().Version?.ToString() ?? "Version not found";
            return Ok(new { Version = version });
        }
        catch (Exception ex)
        {
            _logger.LogError("Error: {Message}", ex.Message);
            return StatusCode(500, new { Error = ex.Message });
        }
    }
}
