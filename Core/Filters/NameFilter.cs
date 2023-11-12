using EUniversity.Core.Models;

namespace EUniversity.Core.Filters;

/// <summary>
/// Filter that filters entities by their names.
/// </summary>
/// <typeparam name="T">The type of entities to filter, which must implement the <see cref="IHasName" />  interface.</typeparam>
public class NameFilter<T> : IFilter<T> where T : IHasName
{
    /// <summary>
    /// Gets the name to filter by.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Initializes a new instance of the NameFilter class with the specified name.
    /// </summary>
    /// <param name="name">The name to filter by.</param>
    public NameFilter(string name)
    {
        Name = name;
    }

    /// <summary>
    /// Apply the name filter to a query.
    /// </summary>
    /// <param name="query">The query that needs to be filtered.</param>
    /// <returns>
    /// Filtered query that contains entities with a matched name.
    /// </returns>
    public IQueryable<T> Apply(IQueryable<T> query)
    {
        return query.Where(x => x.Name.Contains(Name));
    }
}
