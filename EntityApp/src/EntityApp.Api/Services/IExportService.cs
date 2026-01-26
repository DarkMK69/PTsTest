using EntityApp.Api.Dtos;

namespace EntityApp.Api.Services;

public enum ExportFormat
{
    Json,
    Csv,
    Excel
}

public interface IExportService
{
    /// <summary>
    /// Экспортирует сущности в указанном формате
    /// </summary>
    Task<byte[]> ExportEntitiesAsync(List<EntityDto> entities, ExportFormat format);
    
    /// <summary>
    /// Экспортирует одну сущность в указанном формате
    /// </summary>
    Task<byte[]> ExportEntityAsync(EntityDto entity, ExportFormat format);
    
    /// <summary>
    /// Получает расширение файла для формата экспорта
    /// </summary>
    string GetFileExtension(ExportFormat format);
    
    /// <summary>
    /// Получает MIME-тип для формата экспорта
    /// </summary>
    string GetMimeType(ExportFormat format);
}
