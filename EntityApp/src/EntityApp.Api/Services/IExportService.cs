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
    /// Экспортирует и отправляет сущности на mock service
    /// </summary>
    Task<ExportResult> ExportAndSendToMockServiceAsync(List<EntityDto> entities, ExportFormat format);
    
    /// <summary>
    /// Получает MIME-тип для формата экспорта
    /// </summary>
    string GetMimeType(ExportFormat format);
}

public class ExportResult
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public ExportFormat Format { get; set; }
    public int Count { get; set; }
    public string? Error { get; set; }
}
