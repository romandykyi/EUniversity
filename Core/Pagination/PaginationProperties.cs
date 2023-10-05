namespace EUniversity.Core.Pagination
{
    public record struct PaginationProperties(int Page = 1, int PageSize = 20)
    {
        public const int MinPageSize = 5;
        public const int MaxPageSize = 100;
    }
}
