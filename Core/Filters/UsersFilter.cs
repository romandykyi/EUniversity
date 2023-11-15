using EUniversity.Core.Models;

namespace EUniversity.Core.Filters;

/// <summary>
/// Filter for users.
/// </summary>
public class UsersFilter : IFilter<ApplicationUser>
{
    public UsersFilterProperties Properties { get; }

    /// <summary>
    /// Initializes a new instance of the class with the specified properties.
    /// </summary>
    /// <param name="properties">Properties of filtering.</param>
    public UsersFilter(UsersFilterProperties properties)
    {
        Properties = properties;
    }

    /// <summary>
    /// Apply the users filter to a query.
    /// </summary>
    /// <param name="query">The query that needs to be filtered.</param>
    /// <returns>
    /// Filtered query that contains entities which satisfy filter properties.
    /// </returns>
    public IQueryable<ApplicationUser> Apply(IQueryable<ApplicationUser> query)
    {
        throw new NotImplementedException();
    }
}
