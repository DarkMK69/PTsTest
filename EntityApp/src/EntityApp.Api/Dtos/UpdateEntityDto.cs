
using EntityApp.Api.Models;
namespace EntityApp.Api.Dtos;

public record UpdateEntityDto(
    string? Name = null,
    string? Description = null,
    int? Quantity = null,
    decimal? Price = null,
    bool? IsActive = null,
    PriorityLevel? Priority = null,
    List<string>? Tags = null,
    Dictionary<string, string>? Metadata = null,
    double? Rating = null,
    long? Counter = null,
    string? Website = null,
    DateOnly? BirthDate = null,
    TimeOnly? AlarmTime = null,
    Guid? RefId = null,
    string? Email = null,
    string? PhoneNumber = null
);