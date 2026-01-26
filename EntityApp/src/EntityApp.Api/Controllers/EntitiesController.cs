using EntityApp.Api.Dtos;
using EntityApp.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace EntityApp.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EntitiesController(IEntityService service, IExportService exportService, ILogger<EntitiesController> logger) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<PagedResult<EntityDto>>> Get(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10)
    {
        if (page < 1) return BadRequest("Page must be at least 1");
        if (pageSize < 1) return BadRequest("PageSize must be at least 1");
        if (pageSize > 100) return BadRequest("PageSize cannot exceed 100");

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
    public async Task<IActionResult> ExportAll([FromQuery] string format = "json")
    {
        try
        {
            if (!Enum.TryParse<ExportFormat>(format, ignoreCase: true, out var exportFormat))
            {
                return BadRequest(new { error = $"Неподдерживаемый формат: {format}. Доступные форматы: json, csv, excel" });
            }

            var entities = await service.GetAllEntitiesAsync();
            
            if (entities.Count == 0)
            {
                return BadRequest(new { error = "Нет сущностей для экспорта" });
            }

            var fileContent = await exportService.ExportEntitiesAsync(entities, exportFormat);

            using var httpClient = new HttpClient();
            using var content = new ByteArrayContent(fileContent);
            content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(exportService.GetMimeType(exportFormat));

            var response = await httpClient.PostAsync("http://localhost:5001/api/import", content);
            response.EnsureSuccessStatusCode();

            return Ok(new 
            { 
                message = "Экспортировано успешно на mock service",
                format = format,
                count = entities.Count,
                mimeType = exportService.GetMimeType(exportFormat)
            });
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Ошибка при экспорте");
            return StatusCode(500, new { error = "Ошибка при экспорте данных" });
        }
    }
}