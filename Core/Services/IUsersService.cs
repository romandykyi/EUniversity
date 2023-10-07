using EUniversity.Core.Dtos.Users;
using EUniversity.Core.Pagination;

namespace EUniversity.Core.Services
{
    /// <summary>
    /// Service for finding users.
    /// </summary>
    public interface IUsersService
    {
        /// <summary>
        /// Gets all users with the role.
        /// </summary>
        /// <param name="role">Name of the role.</param>
        /// <returns>
        /// Page with all users with the role.
        /// </returns>
        Task<IEnumerable<UserViewDto>> GetUsersInRoleAsync(string role, PaginationProperties? properties);

        /// <summary>
        /// Gets all users at the page.
        /// </summary>
        /// <returns>
        /// Page with all users.
        /// </returns>
        Task<IEnumerable<UserViewDto>> GetAllUsersAsync(PaginationProperties? properties);
    }
}
