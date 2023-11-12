namespace EUniversity.Core.Pagination;

/// <summary>
/// Represents the pagination properties for a collection of items.
/// </summary>
/// <param name="Page">The page number. Default is 1.</param>
/// <param name="PageSize">The number of items per page. Default is 20.</param>
public record PaginationProperties(int Page = 1, int PageSize = 20)
{
    public const int MinPageSize = 5;
    public const int MaxPageSize = 100;
}
