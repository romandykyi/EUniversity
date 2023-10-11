namespace EUniversity.Core.Pagination
{
    /// <summary>
    /// A class that represents a single page with items.
    /// </summary>
    public class Page<TItem>
    {
        /// <summary>
        /// Number of the page(starting from one).
        /// </summary>
        public int PageNumber { get; set; }
        /// <summary>
        /// Maximum number of items this page can contain.
        /// </summary>
        public int PageSize { get; set; }
        /// <summary>
        /// Total number of items.
        /// </summary>
        public int TotalItemsCount { get; set; }
        /// <summary>
        /// Items at this page.
        /// </summary>
        public IEnumerable<TItem> Items { get; set; } = null!;

        public Page() { }

        public Page(IEnumerable<TItem> items, PaginationProperties properties, int totalItemsCount)
        {
            PageNumber = properties.Page;
            PageSize = properties.PageSize;
            Items = items;
            TotalItemsCount = totalItemsCount;
        }
    }
}
