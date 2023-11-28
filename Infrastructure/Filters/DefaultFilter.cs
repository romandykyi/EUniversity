using EUniversity.Core.Filters;
using EUniversity.Core.Models;

namespace EUniversity.Infrastructure.Filters;

/// <summary>
/// Represents a sorting mode for <see cref="DefaultFilter{T}" />.
/// </summary>
public enum DefaultFilterSortingMode
{
    /// <summary>
    /// Default sorting mode, entities will not be sorted.
    /// </summary>
    Default = 0,
    /// <summary>
    /// Sort entities by name alphabetically from a to z.
    /// </summary>
    Name = 1,
    /// <summary>
    /// Sort entities by name alphabetically from z to a.
    /// </summary>
    NameDescending = 2,
    /// <summary>
    /// Sort entities by creation date(newly created entities come first).
    /// </summary>
    Newest = 3,
    /// <summary>
    /// Sort entities by creation date(newly created entities come last).
    /// </summary>
    Oldest = 4
}

/// <summary>
/// Default filter that can be used to filter entities by their name
/// and/or sort them by name or creation date.
/// </summary>
/// <typeparam name="T">
/// The type of entities to filter, which must implement 
/// <see cref="IHasName" /> and <see cref="IHasCreationDate" /> interfaces.
/// </typeparam>
public class DefaultFilter<T> : IFilter<T>
    where T : IHasName, IHasCreationDate
{
    /// <summary>
    /// Gets the name to filter by(if entity doesn't 
    /// implement <see cref="IHasName"/> then this property is ignored).
    /// </summary>
    public string Name { get; }
    /// <summary>
    /// Gets the sorting mode.
    /// </summary>
    public DefaultFilterSortingMode SortingMode { get; }

    /// <summary>
    /// Initializes a new instance of the DefaultFilter class.
    /// </summary>
    /// <param name="name">The name to filter by.</param>
    /// <param name="sortingMode">The sorting mode which will be used.</param>
    public DefaultFilter(string name, DefaultFilterSortingMode sortingMode = DefaultFilterSortingMode.Default)
    {
        Name = name;
        SortingMode = sortingMode;
    }

    /// <summary>
    /// Apply the filter to a query.
    /// </summary>
    /// <param name="query">The query that needs to be filtered.</param>
    /// <returns>
    /// Filtered and sorted query that contains entities with a matched name.
    /// </returns>
    public IQueryable<T> Apply(IQueryable<T> query)
    {
        // Apply sorting mode
        switch (SortingMode)
        {
            case DefaultFilterSortingMode.Name:
                query = query.OrderBy(x => x.Name);
                break;
            case DefaultFilterSortingMode.NameDescending:
                query = query.OrderByDescending(x => x.Name);
                break;
            case DefaultFilterSortingMode.Newest:
                query = query.OrderByDescending(x => x.CreationDate);
                break;
            case DefaultFilterSortingMode.Oldest:
                query = query.OrderBy(x => x.CreationDate);
                break;
        }

        // Filter by name
        return query.Where(x => x.Name.Contains(Name));
    }
}
