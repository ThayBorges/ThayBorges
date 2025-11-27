namespace WasteManagement.Application.Contracts.Common;

public record PaginatedResponse<T>(IReadOnlyCollection<T> Items, PaginationMeta Meta);

public record PaginationMeta
{
    public required int Page { get; init; }
    public required int PageSize { get; init; }
    public required long TotalItems { get; init; }
    public required int TotalPages { get; init; }
    public bool HasNext => Page < TotalPages;
    public bool HasPrevious => Page > 1;
}
