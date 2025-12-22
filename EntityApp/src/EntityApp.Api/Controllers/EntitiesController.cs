using EntityApp.Api.Dtos;
using EntityApp.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace EntityApp.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EntitiesController(IEntityService service, ILogger<EntitiesController> logger) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<PagedResult<EntityDto>>> Get(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10)
    {
        if (page < 1) page = 1;
        if (pageSize < 1) pageSize = 10;
        if (pageSize > 100) pageSize = 100;

        var result = await service.GetEntitiesAsync(page, pageSize);
        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<EntityDto>> Get(Guid id)
    {
        var entity = await service.GetEntityAsync(id);
        return entity is null ? NotFound() : Ok(entity);
    }

    [HttpPost]
    public async Task<ActionResult<EntityDto>> Post(CreateEntityDto dto)
    {
        var created = await service.CreateEntityAsync(dto);
        return CreatedAtAction(nameof(Get), new { id = created.Id }, created);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Put(Guid id, UpdateEntityDto dto)
    {
        var updated = await service.UpdateEntityAsync(id, dto);
        return updated is null ? NotFound() : Ok(updated);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var success = await service.DeleteEntityAsync(id);
        return success ? NoContent() : NotFound();
    }

    [HttpPost("export")]
    public async Task<IActionResult> ExportAll()
    {
        var entities = await service.GetAllEntitiesAsync();
        using var httpClient = new HttpClient();
        try
        {
            var json = System.Text.Json.JsonSerializer.Serialize(entities);
            var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync("http://localhost:5001/api/import", content);
            response.EnsureSuccessStatusCode();
            return Ok("Exported successfully to mock service");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Export failed");
            return StatusCode(500, "Export failed");
        }
    }
}