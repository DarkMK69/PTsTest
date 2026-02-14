namespace EntityApp.Api.Services;

/// <summary>
/// Реализация сервиса для отправки экспортированных данных на webhook/API клиента
/// </summary>
public class ReportWebhookSender : IReportWebhookSender
{
    private readonly ILogger<ReportWebhookSender> _logger;
    private readonly IEntityFormatter _formatter;

    public ReportWebhookSender(ILogger<ReportWebhookSender> logger, IEntityFormatter formatter)
    {
        _logger = logger;
        _formatter = formatter;
    }

    public async Task<bool> SendExportDataAsync(byte[] fileContent, ExportFormat format, string endpoint)
    {
        try
        {
            using var httpClient = new HttpClient();
            using var content = new ByteArrayContent(fileContent);
            content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(_formatter.GetMimeType(format));

            var response = await httpClient.PostAsync(endpoint, content);
            response.EnsureSuccessStatusCode();

            _logger.LogInformation("Данные успешно отправлены на {Endpoint}", endpoint);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка при отправке данных на {Endpoint}", endpoint);
            return false;
        }
    }
}
