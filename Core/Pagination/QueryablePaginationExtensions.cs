using Mapster;

namespace EUniversity.Core.Pagination
{
    public static class QueryablePaginationExtensions
    {
        /// <summary>
        /// Applies pagination to an <see cref="IQueryable{TItem}"/>.
        /// </summary>
        /// <typeparam name="TItem">The type of elements in the queryable.</typeparam>
        /// <param name="queryable">The source <see cref="IQueryable{TItem}"/> to apply pagination to.</param>
        /// <param name="properties"><see cref="PaginationProperties"/> object specifying pagination parameters.</param>
        /// <returns>An <see cref="IQueryable{TItem}"/> representing the paginated subset of the source queryable.</returns>
        public static IQueryable<TItem> ApplyPagination<TItem>(
            this IQueryable<TItem> queryable, PaginationProperties properties)
        {
            return queryable
                .Skip((properties.Page - 1) * properties.PageSize)
                .Take(properties.PageSize);
        }

        /// <summary>
        /// Applies pagination and returns the results as a page of <typeparamref name="TDto"/>.
        /// </summary>
        /// <typeparam name="TItem">The type of elements in the source queryable.</typeparam>
        /// <typeparam name="TDto">The type to which the results are projected.</typeparam>
        /// <param name="queryable">The source <see cref="IQueryable{TItem}"/> to apply pagination to.</param>
        /// <param name="properties">An optional <see cref="PaginationProperties"/> object specifying pagination parameters. If <see langword="null" />, default parameters are used.</param>
        /// <returns>A page of items of type <typeparamref name="TDto"/> from the queryable.</returns>
        public static async Task<Page<TDto>> ToPageAsync<TItem, TDto>(
            this IQueryable<TItem> queryable, PaginationProperties? properties = null)
        {
            // If properties is null, then use default
            properties ??= new();

            // Get items at the page
            var items = await queryable
                .ApplyPagination(properties)
                .ProjectToType<TDto>()
                .ToListAsync();
            // Count all items
            int count = await queryable.CountAsync();

            return new Page<TDto>(items, properties, count);
        }
    }
}
