using EUniversity.Core.Filters;
using EUniversity.Core.Models;
using System.Linq.Expressions;

namespace EUniversity.Infrastructure.Filters;

/// <summary>
/// Filter for users.
/// </summary>
public class UsersFilter : IFilter<ApplicationUser>
{
    public UsersFilterProperties Properties { get; }

    private Expression<Func<ApplicationUser, string>> FullNameKeySelector =
        u => u.FirstName + ' ' + (u.MiddleName != null ? u.MiddleName + ' ' : "") + u.LastName;

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
        // Filtering
        if (Properties.FullName != null)
        {
            query = query.Where(
                u => (u.FirstName + ' ' + (u.MiddleName != null ? u.MiddleName + ' ' : "") + u.LastName)
                .Contains(Properties.FullName)
                );
        }
        if (Properties.Email != null)
        {
            query = query.Where(u => u.Email != null && u.Email.Contains(Properties.Email));
        }
        if (Properties.UserName != null)
        {
            query = query.Where(u => u.UserName != null && u.UserName.Contains(Properties.UserName));
        }

        // Sorting
        switch (Properties.SortingMode)
        {
            case UsersSortingMode.FullName:
                query = query.OrderBy(FullNameKeySelector);
                break;
            case UsersSortingMode.FullNameDescending:
                query = query.OrderByDescending(FullNameKeySelector);
                break;
            case UsersSortingMode.UserName:
                query = query.OrderBy(u => u.UserName);
                break;
            case UsersSortingMode.UserNameDescending:
                query = query.OrderByDescending(u => u.UserName);
                break;
        }

        return query;
    }
}
