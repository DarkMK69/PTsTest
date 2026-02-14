namespace EntityApp.Api.Services;

/// <summary>
/// Сервис для отправки экспортированных данных на webhook/API клиента
/// </summary>
public interface IReportWebhookSender
{
    /// <summary>
    /// Отправляет данные на указанный endpoint
    /// </summary>
    Task<bool> SendExportDataAsync(byte[] fileContent, ExportFormat format, string endpoint);
}
