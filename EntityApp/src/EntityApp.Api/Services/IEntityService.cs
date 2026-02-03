using EntityApp.Api.Dtos;

namespace EntityApp.Api.Services;

public interface IEntityService
{
    /// <summary>
    /// Получает постраничный список сущностей.
    /// </summary>
    /// <param name="pageNumber">Номер страницы (начиная с 1)</param>
    /// <param name="pageSize">Размер страницы</param>
    /// <returns>Постраничный результат со списком DTO сущностей</returns>
    Task<PagedResult<EntityDto>> GetEntitiesAsync(int pageNumber, int pageSize);

    /// <summary>
    /// Получает сущность по идентификатору.
    /// </summary>
    /// <param name="id">Уникальный идентификатор сущности</param>
    /// <returns>DTO сущности или null, если не найдена</returns>
    Task<EntityDto?> GetEntityAsync(Guid id);

    /// <summary>
    /// Создает новую сущность.
    /// </summary>
    /// <param name="dto">DTO с данными для создания сущности</param>
    /// <returns>DTO созданной сущности с присвоенным идентификатором</returns>
    Task<EntityDto> CreateEntityAsync(CreateEntityDto dto);

    /// <summary>
    /// Обновляет существующую сущность.
    /// </summary>
    /// <param name="id">Идентификатор сущности для обновления</param>
    /// <param name="dto">DTO с новыми данными</param>
    /// <returns>DTO обновленной сущности или null, если сущность не найдена</returns>
    Task<EntityDto?> UpdateEntityAsync(Guid id, UpdateEntityDto dto);

    /// <summary>
    /// Удаляет сущность по идентификатору.
    /// </summary>
    /// <param name="id">Идентификатор сущности для удаления</param>
    /// <returns>true, если сущность была удалена; false, если не найдена</returns>
    Task<bool> DeleteEntityAsync(Guid id);

    /// <summary>
    /// Получает все сущности без пагинации.
    /// </summary>
    /// <returns>Список всех DTO сущностей</returns>
    Task<List<EntityDto>> GetAllEntitiesAsync();
}