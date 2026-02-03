namespace EntityApp.Api.Services;

/// <summary>
/// Результат постраничной выборки данных.
/// </summary>
/// <typeparam name="T">Тип элементов в результате</typeparam>
public class PagedResult<T>
{
    /// <summary>
    /// Список элементов текущей страницы.
    /// </summary>
    public List<T> Items { get; set; } = [];

    /// <summary>
    /// Номер текущей страницы.
    /// </summary>
    public int PageNumber { get; set; }

    /// <summary>
    /// Размер страницы.
    /// </summary>
    public int PageSize { get; set; }

    /// <summary>
    /// Общее количество элементов.
    /// </summary>
    public int TotalCount { get; set; }

    /// <summary>
    /// Общее количество страниц.
    /// </summary>
    public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
}
