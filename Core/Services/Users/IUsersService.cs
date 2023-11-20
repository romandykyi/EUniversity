using EUniversity.Core.Dtos.Users;
using EUniversity.Core.Filters;
using EUniversity.Core.Models;
using EUniversity.Core.Pagination;

namespace EUniversity.Core.Services.Users;

/// <summary>
/// Service for finding users.
/// </summary>
public interface IUsersService
{
    /// <summary>
    /// Gets a page with all users that have the role.
    /// </summary>
    /// <param name="role">Name of the role.</param>
    /// <param name="properties">Optional pagination properties to use.</param>
    /// <param name="filter">Optional filter to be applied.</param>
    /// <returns>
    /// Page with all users with the role.
    /// </returns>
    Task<Page<UserViewDto>> GetUsersInRoleAsync(string role, PaginationProperties? properties = null,
        IFilter<ApplicationUser>? filter = null);

    /// <summary>
    /// Gets a page with all users.
    /// </summary>
    /// <param name="properties">Optional pagination properties to use.</param>
    /// <param name="filter">Optional filter to be applied.</param>
    /// <returns>
    /// Page with all users.
    /// </returns>
    Task<Page<UserViewDto>> GetAllUsersAsync(PaginationProperties? properties = null, IFilter<ApplicationUser>? filter = null);
}
