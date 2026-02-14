using EntityApp.Api.Dtos;

namespace EntityApp.Api.Services;

public interface IExportService
{
    /// <summary>
    /// Экспортирует и отправляет сущности на mock service
    /// </summary>
    Task<ExportResult> ExportAndSendToMockServiceAsync( ExportFormat format);
    
    /// <summary>
    /// Получает MIME-тип для формата экспорта
    /// </summary>
    string GetMimeType(ExportFormat format);
}
