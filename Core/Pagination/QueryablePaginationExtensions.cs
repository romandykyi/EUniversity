namespace EUniversity.Core.Pagination
{
    public static class QueryablePaginationExtensions
    {
        public static IQueryable<T> ApplyPagination<T>(
            this IQueryable<T> queryable, PaginationProperties? properties)
        {
            properties ??= new();
            return queryable
                .Skip((properties.Page - 1) * properties.PageSize)
                .Take(properties.PageSize);
        }
    }
}
