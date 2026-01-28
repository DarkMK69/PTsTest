using EntityApp.Api.Dtos;

namespace EntityApp.Api.Services;

public class ExportService : IExportService
{
    private readonly ILogger<ExportService> _logger;
    private readonly IEntityService _entityService;
    private readonly IReportWebhookSender _webhookSender;
    private readonly IEntityFormatter _formatter;
    private const string MockServiceEndpoint = "http://localhost:5001/api/import";

    public ExportService(
        ILogger<ExportService> logger,
        IEntityService entityService,
        IReportWebhookSender webhookSender,
        IEntityFormatter formatter)
    {
        _logger = logger;
        _entityService = entityService;
        _webhookSender = webhookSender;
        _formatter = formatter;
    }

    public string GetMimeType(ExportFormat format)
    {
        return _formatter.GetMimeType(format);
    }

    public async Task<ExportResult> ExportAndSendToMockServiceAsync(List<EntityDto> entities, ExportFormat format)
    {
        if (!Enum.IsDefined(typeof(ExportFormat), format))
        {
            return new ExportResult
            {
                Success = false,
                Error = $"Неподдерживаемый формат: {format}. Доступные форматы: json, csv, excel",
                Format = format
            };
        }

        if (entities.Count == 0)
        {
            return new ExportResult
            {
                Success = false,
                Error = "Нет сущностей для экспорта",
                Format = format
            };
        }

        try
        {
            var fileContent = await _formatter.FormatEntitiesAsync(entities, format);
            var sent = await _webhookSender.SendExportDataAsync(fileContent, format, MockServiceEndpoint);

            if (!sent)
            {
                return new ExportResult
                {
                    Success = false,
                    Error = "Ошибка при отправке данных на mock service",
                    Format = format
                };
            }

            return new ExportResult
            {
                Success = true,
                Message = "Экспортировано успешно на mock service",
                Format = format,
                Count = entities.Count
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка при экспорте и отправке на mock service");
            return new ExportResult
            {
                Success = false,
                Error = "Ошибка при экспорте данных",
                Format = format
            };
        }
    }
}
