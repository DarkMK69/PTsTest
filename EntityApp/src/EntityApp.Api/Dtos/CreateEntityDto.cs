namespace EntityApp.Api.Dtos;
using EntityApp.Api.Models;
public record CreateEntityDto(
    string Name,
    string Description = "",
    int Quantity = 0,
    decimal Price = 0,
    bool IsActive = true,
    PriorityLevel Priority = PriorityLevel.Medium,
    List<string>? Tags = null,
    Dictionary<string, string>? Metadata = null,
    double Rating = 0,
    long Counter = 0,
    string? Website = null,
    DateOnly? BirthDate = null,
    TimeOnly? AlarmTime = null,
    Guid? RefId = null,
    string? Email = null,
    string? PhoneNumber = null
);