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
    Task<Page<UserPreviewDto>> GetUsersInRoleAsync(string role, PaginationProperties? properties = null,
        IFilter<ApplicationUser>? filter = null);

    /// <summary>
    /// Gets a page with all users.
    /// </summary>
    /// <param name="properties">Optional pagination properties to use.</param>
    /// <param name="filter">Optional filter to be applied.</param>
    /// <param name="onlyDeleted">
    /// Optional flag. If <see langword="true"/> then only deleted users 
    /// will be returned, otherwise only not deleted users will be returned.</param>
    /// <returns>
    /// Page with all users.
    /// </returns>
    Task<Page<UserPreviewDto>> GetAllUsersAsync(PaginationProperties? properties = null, 
        IFilter<ApplicationUser>? filter = null, bool onlyDeleted = false);

    /// <summary>
    /// Gets a user by its ID.
    /// </summary>
    /// <param name="id"></param>
    /// <returns>
    /// A task that represents the asynchronous operation.
    /// Returns <see langword="null" /> if user is not found.
    /// </returns>
    Task<UserViewDto?> GetByIdAsync(string id);

    /// <summary>
    /// Deletes a user identified by its unique identifier asynchronously.
    /// </summary>
    /// <remarks>
    /// This method performs a 'soft' deleted after which user is still remains in
    /// the database but its 'IsDeleted' flag is set to <see langword="true"/>.
    /// </remarks>
    /// <param name="id">The unique identifier of the user to delete.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. If the entity 
    /// is deleted successfully, it returns <see langword="true" />.
    /// If the user with the specified identifier is not found, 
    /// it returns <see langword="false" />.
    /// </returns>
    Task<bool> DeleteUserAsync(string userId);

    /// <summary>
    /// Updates an existing user identified by its unique identifier asynchronously.
    /// </summary>
    /// <param name="id">The unique identifier of the user to update.</param>
    /// <param name="dto">The DTO containing data for updating the user.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. If the 
    /// user is updated successfully, it returns <see langword="true" />.
    /// If the user with the specified identifier is not found(or deleted), 
    /// it returns <see langword="false" />.
    /// </returns>
    Task<bool> UpdateUserAsync(string userId, EditUserDto editUserDto);
}
