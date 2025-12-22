namespace EntityApp.Api.Dtos;
using EntityApp.Api.Models;
public record EntityDto(
    Guid Id,
    string Name,
    string Description,
    int Quantity,
    decimal Price,
    DateTime CreatedAt,
    DateTime? UpdatedAt,
    bool IsActive,
    PriorityLevel Priority,
    List<string> Tags,
    Dictionary<string, string> Metadata,
    double Rating,
    long Counter,
    string? Website,
    DateOnly? BirthDate,
    TimeOnly? AlarmTime,
    Guid? RefId,
    string? Email,
    string? PhoneNumber
);