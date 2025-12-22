using EntityApp.Api.Dtos;
using EntityApp.Api.Models;

namespace EntityApp.Api.Services;

public interface IEntityService
{
    Task<PagedResult<EntityDto>> GetEntitiesAsync(int pageNumber, int pageSize);
    Task<EntityDto?> GetEntityAsync(Guid id);
    Task<EntityDto> CreateEntityAsync(CreateEntityDto dto);
    Task<EntityDto?> UpdateEntityAsync(Guid id, UpdateEntityDto dto);
    Task<bool> DeleteEntityAsync(Guid id);
    Task<List<EntityDto>> GetAllEntitiesAsync();
}

public class PagedResult<T>
{
    public List<T> Items { get; set; } = [];
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int TotalCount { get; set; }
    public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
}

public class EntityService : IEntityService
{
    private readonly List<Entity> _store = [];
    private readonly object _lock = new();

    public EntityService()
    {
        // Seed
        _store.Add(new Entity
        {
            Id = Guid.Parse("11111111-1111-1111-1111-111111111111"),
            Name = "Initial Item",
            CreatedAt = DateTime.UtcNow,
            Priority = PriorityLevel.Medium,
            Tags = ["demo"],
            Metadata = new() { ["env"] = "seed" }
        });
    }

    public Task<List<EntityDto>> GetAllEntitiesAsync()
    {
        var dtos = _store.Select(MapToDto).ToList();
        return Task.FromResult(dtos);
    }

    public Task<PagedResult<EntityDto>> GetEntitiesAsync(int pageNumber, int pageSize)
    {
        var totalCount = _store.Count;
        var items = _store
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .Select(MapToDto)
            .ToList();

        return Task.FromResult(new PagedResult<EntityDto>
        {
            Items = items,
            PageNumber = pageNumber,
            PageSize = pageSize,
            TotalCount = totalCount
        });
    }

    public Task<EntityDto?> GetEntityAsync(Guid id)
    {
        var entity = _store.FirstOrDefault(e => e.Id == id);
        return Task.FromResult(entity == null ? null : MapToDto(entity));
    }

    public Task<EntityDto> CreateEntityAsync(CreateEntityDto dto)
    {
        var entity = new Entity
        {
            Id = Guid.NewGuid(),
            Name = dto.Name,
            Description = dto.Description,
            Quantity = dto.Quantity,
            Price = dto.Price,
            CreatedAt = DateTime.UtcNow,
            IsActive = dto.IsActive,
            Priority = dto.Priority,
            Tags = dto.Tags ?? [],
            Metadata = dto.Metadata ?? [],
            Rating = dto.Rating,
            Counter = dto.Counter,
            Website = dto.Website,
            BirthDate = dto.BirthDate,
            AlarmTime = dto.AlarmTime,
            RefId = dto.RefId,
            Email = dto.Email,
            PhoneNumber = dto.PhoneNumber
        };

        lock (_lock) _store.Add(entity);
        return Task.FromResult(MapToDto(entity));
    }

    public Task<EntityDto?> UpdateEntityAsync(Guid id, UpdateEntityDto dto)
    {
        var index = _store.FindIndex(e => e.Id == id);
        if (index == -1) return Task.FromResult<EntityDto?>(null);

        var existing = _store[index];
        if (dto.Name != null) existing.Name = dto.Name;
        if (dto.Description != null) existing.Description = dto.Description;
        if (dto.Quantity != null) existing.Quantity = dto.Quantity.Value;
        if (dto.Price != null) existing.Price = dto.Price.Value;
        if (dto.IsActive != null) existing.IsActive = dto.IsActive.Value;
        if (dto.Priority != null) existing.Priority = dto.Priority.Value;
        if (dto.Tags != null) existing.Tags = dto.Tags;
        if (dto.Metadata != null) existing.Metadata = dto.Metadata;
        if (dto.Rating != null) existing.Rating = dto.Rating.Value;
        if (dto.Counter != null) existing.Counter = dto.Counter.Value;
        if (dto.Website != null) existing.Website = dto.Website;
        if (dto.BirthDate != null) existing.BirthDate = dto.BirthDate.Value;
        if (dto.AlarmTime != null) existing.AlarmTime = dto.AlarmTime.Value;
        if (dto.RefId != null) existing.RefId = dto.RefId.Value;
        if (dto.Email != null) existing.Email = dto.Email;
        if (dto.PhoneNumber != null) existing.PhoneNumber = dto.PhoneNumber;

        existing.UpdatedAt = DateTime.UtcNow;

        var updated = MapToDto(existing);
        return Task.FromResult<EntityDto?>(updated);
    }

    public Task<bool> DeleteEntityAsync(Guid id)
    {
        lock (_lock)
        {
            var removed = _store.RemoveAll(e => e.Id == id) > 0;
            return Task.FromResult(removed);
        }
    }

    private static EntityDto MapToDto(Entity e) => new(
        e.Id,
        e.Name,
        e.Description,
        e.Quantity,
        e.Price,
        e.CreatedAt,
        e.UpdatedAt,
        e.IsActive,
        e.Priority,
        e.Tags,
        e.Metadata,
        e.Rating,
        e.Counter,
        e.Website,
        e.BirthDate,
        e.AlarmTime,
        e.RefId,
        e.Email,
        e.PhoneNumber
    );
}