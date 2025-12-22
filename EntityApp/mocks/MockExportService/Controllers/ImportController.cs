using Microsoft.AspNetCore.Mvc;

namespace MockExportService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ImportController(ILogger<ImportController> logger) : ControllerBase
{
    [HttpPost]
    public IActionResult Import([FromBody] List<object> entities)
    {
        logger.LogInformation("✅ Received {Count} entities for import", entities?.Count ?? 0);
        return Ok(new { message = "Import successful", count = entities?.Count ?? 0 });
    }
}