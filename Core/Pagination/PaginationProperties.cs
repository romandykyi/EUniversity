namespace EUniversity.Core.Pagination
{
    public record PaginationProperties(int Page = 1, int PageSize = 20)
    {
        public const int MinPageSize = 5;
        public const int MaxPageSize = 100;
    }
}
