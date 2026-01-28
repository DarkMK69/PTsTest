using EntityApp.Api.Dtos;
using EntityApp.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace EntityApp.Api.Controllers;

/// <summary>
/// Контроллер для управления сущностями (Entity).
/// Предоставляет REST API для CRUD операций и экспорта данных.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class EntitiesController(IEntityService service, IExportService exportService) : ControllerBase
{
    /// <summary>
    /// Получает постраничный список всех сущностей.
    /// </summary>
    /// <param name="page">Номер страницы (по умолчанию 1)</param>
    /// <param name="pageSize">Размер страницы (по умолчанию 10, максимум 100)</param>
    /// <returns>Постраничный результат со списком сущностей</returns>
    /// <response code="200">Успешно получен список сущностей</response>
    /// <response code="400">Некорректные параметры пагинации</response>
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

    /// <summary>
    /// Получает сущность по идентификатору.
    /// </summary>
    /// <param name="id">Уникальный идентификатор сущности</param>
    /// <returns>Данные сущности</returns>
    /// <response code="200">Сущность успешно найдена</response>
    /// <response code="404">Сущность не найдена</response>
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<EntityDto>> Get(Guid id)
    {
        var entity = await service.GetEntityAsync(id);
        return entity is null ? NotFound() : Ok(entity);
    }

    /// <summary>
    /// Создает новую сущность.
    /// </summary>
    /// <param name="dto">Данные новой сущности</param>
    /// <returns>Созданная сущность с идентификатором</returns>
    /// <response code="201">Сущность успешно создана</response>
    /// <response code="400">Некорректные данные</response>
    [HttpPost]
    public async Task<ActionResult<EntityDto>> Post(CreateEntityDto dto)
    {
        var created = await service.CreateEntityAsync(dto);
        return CreatedAtAction(nameof(Get), new { id = created.Id }, created);
    }

    /// <summary>
    /// Обновляет существующую сущность.
    /// </summary>
    /// <param name="id">Идентификатор сущности для обновления</param>
    /// <param name="dto">Новые данные сущности</param>
    /// <returns>Обновленная сущность</returns>
    /// <response code="200">Сущность успешно обновлена</response>
    /// <response code="404">Сущность не найдена</response>
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Put(Guid id, UpdateEntityDto dto)
    {
        var updated = await service.UpdateEntityAsync(id, dto);
        return updated is null ? NotFound() : Ok(updated);
    }

    /// <summary>
    /// Удаляет сущность.
    /// </summary>
    /// <param name="id">Идентификатор сущности для удаления</param>
    /// <returns>Статус удаления</returns>
    /// <response code="204">Сущность успешно удалена</response>
    /// <response code="404">Сущность не найдена</response>
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var success = await service.DeleteEntityAsync(id);
        return success ? NoContent() : NotFound();
    }

    /// <summary>
    /// Экспортирует все сущности в указанном формате и отправляет на mock service.
    /// </summary>
    /// <param name="format">Формат экспорта: json, csv, excel (по умолчанию json)</param>
    /// <returns>Результат экспорта с информацией о количестве и формате</returns>
    /// <response code="200">Данные успешно экспортированы</response>
    /// <response code="400">Некорректный формат или нет данных для экспорта</response>
    /// <response code="500">Ошибка при экспорте или отправке на mock service</response>
    [HttpPost("export")]
    public async Task<IActionResult> ExportAll([FromQuery] ExportFormat format = ExportFormat.Json)
    {
        var entities = await service.GetAllEntitiesAsync();
        var result = await exportService.ExportAndSendToMockServiceAsync(entities, format);

        if (!result.Success)
        {
            return result.Error?.Contains("Нет сущностей") == true 
                ? BadRequest(new { error = result.Error })
                : StatusCode(500, new { error = result.Error });
        }

        return Ok(new 
        { 
            message = result.Message,
            format = result.Format,
            count = result.Count,
            mimeType = exportService.GetMimeType(result.Format)
        });
    }
}