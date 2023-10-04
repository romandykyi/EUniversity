using EUniversity.Core.Dtos.Users;

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
        /// <see cref="UsersViewDto"/> representing all users with the role.
        /// </returns>
        Task<UsersViewDto> GetUsersInRoleAsync(string role);

        /// <summary>
        /// Gets all users.
        /// </summary>
        /// <returns>
        /// <see cref="UsersViewDto"/> representing all users.
        /// </returns>
        Task<UsersViewDto> GetAllUsersAsync();
    }
}
