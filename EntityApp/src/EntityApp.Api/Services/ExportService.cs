using System.Globalization;
using System.Text;
using System.Text.Json;
using CsvHelper;
using EntityApp.Api.Dtos;

namespace EntityApp.Api.Services;

public class ExportService : IExportService
{
    private readonly ILogger<ExportService> _logger;

    public ExportService(ILogger<ExportService> logger)
    {
        _logger = logger;
    }

    public async Task<byte[]> ExportEntitiesAsync(List<EntityDto> entities, ExportFormat format)
    {
        try
        {
            return format switch
            {
                ExportFormat.Json => ExportToJson(entities),
                ExportFormat.Csv => ExportToCsv(entities),
                ExportFormat.Excel => await ExportToExcelAsync(entities),
                _ => throw new ArgumentException($"Неподдерживаемый формат: {format}")
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка при экспорте сущностей в формате {Format}", format);
            throw;
        }
    }

    public async Task<byte[]> ExportEntityAsync(EntityDto entity, ExportFormat format)
    {
        return await ExportEntitiesAsync(new List<EntityDto> { entity }, format);
    }

    public string GetFileExtension(ExportFormat format)
    {
        return format switch
        {
            ExportFormat.Json => ".json",
            ExportFormat.Csv => ".csv",
            ExportFormat.Excel => ".xlsx",
            _ => throw new ArgumentException($"Неподдерживаемый формат: {format}")
        };
    }

    public string GetMimeType(ExportFormat format)
    {
        return format switch
        {
            ExportFormat.Json => "application/json",
            ExportFormat.Csv => "text/csv",
            ExportFormat.Excel => "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            _ => throw new ArgumentException($"Неподдерживаемый формат: {format}")
        };
    }

    private byte[] ExportToJson(List<EntityDto> entities)
    {
        var options = new JsonSerializerOptions
        {
            WriteIndented = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        var json = JsonSerializer.Serialize(entities, options);
        return Encoding.UTF8.GetBytes(json);
    }

    private byte[] ExportToCsv(List<EntityDto> entities)
    {
        using var memoryStream = new MemoryStream();
        using var writer = new StreamWriter(memoryStream, Encoding.UTF8);
        using var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);

        csv.WriteRecords(entities);
        writer.Flush();

        return memoryStream.ToArray();
    }

    private async Task<byte[]> ExportToExcelAsync(List<EntityDto> entities)
    {
        // Для простой реализации используем CSV в формате Excel
        // Если нужен полноценный Excel, потребуется установить пакет EPPlus или подобный
        _logger.LogWarning("Excel экспорт временно недоступен. Используется CSV формат.");
        return ExportToCsv(entities);
    }
}
