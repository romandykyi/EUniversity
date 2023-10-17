namespace EUniversity.Core.Filters
{
    /// <summary>
    /// Interface that defines a filter for IQueryable.
    /// </summary>
    public interface IFilter<T>
    {
        /// <summary>
        /// Apply the filter to a query.
        /// </summary>
        /// <param name="query">Query that needs to be filtered.</param>
        /// <returns>
        /// Filtered query.
        /// </returns>
        IQueryable<T> Apply(IQueryable<T> query);
    }
}
