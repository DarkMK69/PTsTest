using System.Globalization;
using System.Text;
using System.Text.Json;
using CsvHelper;
using EntityApp.Api.Dtos;

namespace EntityApp.Api.Services;

/// <summary>
/// Сервис для форматирования сущностей в различные форматы экспорта
/// </summary>
public interface IEntityFormatter
{
    /// <summary>
    /// Форматирует сущности в указанном формате
    /// </summary>
    Task<byte[]> FormatEntitiesAsync(List<EntityDto> entities, ExportFormat format);

    /// <summary>
    /// Получает расширение файла для формата экспорта
    /// </summary>
    string GetFileExtension(ExportFormat format);

    /// <summary>
    /// Получает MIME-тип для формата экспорта
    /// </summary>
    string GetMimeType(ExportFormat format);
}

public class EntityFormatter : IEntityFormatter
{
    private readonly ILogger<EntityFormatter> _logger;

    public EntityFormatter(ILogger<EntityFormatter> logger)
    {
        _logger = logger;
    }

    public async Task<byte[]> FormatEntitiesAsync(List<EntityDto> entities, ExportFormat format)
    {
        try
        {
            return format switch
            {
                ExportFormat.Json => FormatToJson(entities),
                ExportFormat.Csv => FormatToCsv(entities),
                ExportFormat.Excel => await FormatToExcelAsync(entities),
                _ => throw new ArgumentException($"Неподдерживаемый формат: {format}")
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка при форматировании сущностей в формате {Format}", format);
            throw;
        }
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

    private byte[] FormatToJson(List<EntityDto> entities)
    {
        var options = new JsonSerializerOptions
        {
            WriteIndented = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        var json = JsonSerializer.Serialize(entities, options);
        return Encoding.UTF8.GetBytes(json);
    }

    private byte[] FormatToCsv(List<EntityDto> entities)
    {
        using var memoryStream = new MemoryStream();
        using var writer = new StreamWriter(memoryStream, Encoding.UTF8);
        using var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);

        csv.WriteRecords(entities);
        writer.Flush();

        return memoryStream.ToArray();
    }

    private async Task<byte[]> FormatToExcelAsync(List<EntityDto> entities)
    {
        // Для простой реализации используем CSV в формате Excel
        // Если нужен полноценный Excel, потребуется установить пакет EPPlus или подобный
        _logger.LogWarning("Excel экспорт временно недоступен. Используется CSV формат.");
        return FormatToCsv(entities);
    }
}
