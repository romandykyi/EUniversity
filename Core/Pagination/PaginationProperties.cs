namespace EUniversity.Core.Pagination
{
    public record struct PaginationProperties(int Page, int PageSize)
    {
        public const int MinPageSize = 5;
        public const int MaxPageSize = 100;
    }
}
