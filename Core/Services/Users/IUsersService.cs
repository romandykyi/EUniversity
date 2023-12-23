using EUniversity.Core.Dtos.University;
using EUniversity.Core.Dtos.Users;
using EUniversity.Core.Filters;
using EUniversity.Core.Models;
using EUniversity.Core.Models.University;
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
    /// <returns>
    /// Page with all users.
    /// </returns>
    Task<Page<UserPreviewDto>> GetAllUsersAsync(PaginationProperties? properties = null, IFilter<ApplicationUser>? filter = null);

    /// <summary>
    /// Gets a page with groups of the student.
    /// </summary>
    /// <param name="studentId">An ID of the student.</param>
    /// <param name="filter">Optional filter for groups.</param>
    /// <returns>
    /// A page with groups of the student with ID <paramref name="studentId"/>.
    /// </returns>
    Task<Page<GroupPreviewDto>> GetGroupsOfStudentAsync(string studentId, PaginationProperties properties, IFilter<Group>? filter = null);

    /// <summary>
    /// Gets a page with semesters of the student.
    /// </summary>
    /// <param name="studentId">An ID of the student.</param>
    /// <param name="filter">Optional filter for semesters.</param>
    /// <returns>
    /// A page with semesters of the student with ID <paramref name="studentId"/>.
    /// </returns>
    Task<Page<SemesterPreviewDto>> GetSemestersOfStudentAsync(string studentId, PaginationProperties properties, IFilter<Semester>? filter = null);
}
