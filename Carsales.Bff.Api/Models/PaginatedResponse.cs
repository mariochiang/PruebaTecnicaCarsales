namespace Carsales.Bff.Api.Models;

public sealed class PaginatedResponse<T>
{
    public int Total { get; init; }
    public int Pages { get; init; }
    public int Page { get; init; }
    public int PageSize { get; init; }
    public IReadOnlyList<T> Items { get; init; } = Array.Empty<T>();
}
