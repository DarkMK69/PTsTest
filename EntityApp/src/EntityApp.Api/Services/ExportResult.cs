namespace EntityApp.Api.Services;

public class ExportResult
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public ExportFormat Format { get; set; }
    public int Count { get; set; }
    public string? Error { get; set; }
}
