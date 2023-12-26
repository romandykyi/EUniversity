using EUniversity.Core.Dtos.University.Grades;
using EUniversity.Core.Filters;
using EUniversity.Core.Models.University.Grades;
using EUniversity.Core.Pagination;

namespace EUniversity.Core.Services.University.Grades;

/// <summary>
/// Response of the <see cref="IGroupsService.GetAssignerIdAsync(int)" /> method.
/// </summary>
/// <param name="GradeExists">Flag that determines whether requested assigned grade exists.</param>
/// <param name="AssignerId">ID of the assigner of the grade.</param>
public record GetAssignerIdResponse(bool GradeExists, string? AssignerId);

/// <summary>
/// An interface for retrieving and assigning grades.
/// </summary>
public interface IAssignedGradesService
{
    /// <summary>
    /// Retrieves a page with grades asynchronously.
    /// </summary>
    /// <remarks>
    /// Requires a map from <typeparamref name="TGrade"/> to <typeparamref name="TViewDto"/>.
    /// </remarks>
    /// <param name="properties"><see cref="PaginationProperties"/> object specifying pagination parameters.</param>
    /// <param name="filter">An optional filter to apply.</param>
    /// <param name="includeGroups">If <see langword="true"/>, then groups of grades will be included.</param>
    /// <param name="includeStudents">If <see langword="true"/>, then students(assignees) will be included.</param>
    /// <typeparam name="TViewDto">A type of returned DTOs.</typeparam>
    /// <returns>
    /// A task that represents the asynchronous operation, containing 
    /// the page with entities previews.
    /// </returns>
    Task<Page<TViewDto>> GetPageAsync<TViewDto>(PaginationProperties properties, IFilter<AssignedGrade>? filter = null,
        bool includeGroups = true, bool includeStudents = true);

    /// <summary>
    /// Assignes a grade to a student based on the information provided in the
    /// <see cref="dto" /> asynchronously.
    /// </summary>
    /// <param name="dto">The DTO containing data for assigning the grade.</param>
    /// <param name="assignerId">ID of the user who assigns the grade.</param>
    /// <returns>
    /// A task that represents the asynchronous operation, containing
    /// the newly created entity.
    /// </returns>
    Task<AssignedGrade> AssignAsync(AssignedGradeCreateDto dto, string assignerId);

    /// <summary>
    /// Reassignes a grade to a student based on the information provided in the
    /// <see cref="dto" /> asynchronously.
    /// </summary>
    /// <param name="id">The unique identifier of the grade to reassign.</param>
    /// <param name="dto">The DTO containing data for reassigning the grade.</param>
    /// <param name="reassignerId">ID of the user who reassigns the grade.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. If the 
    /// grade is reassigned successfully, it returns <see langword="true" />.
    /// If the grade to reassign was not not found, 
    /// it returns <see langword="false" />.
    /// </returns>
    Task<bool> RessignAsync(int id, AssignedGradeUpdateDto dto, string reassignerId);

    /// <summary>
    /// Deletes a grade identified by its unique identifier asynchronously.
    /// </summary>
    /// <param name="id">The unique identifier of the grade to delete.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. If the grade 
    /// is deleted successfully, it returns <see langword="true" />.
    /// If the grade with the specified identifier is not found, 
    /// it returns <see langword="false" />.
    /// </returns>
    Task<bool> DeleteAsync(int id);

    /// <summary>
    /// Gets an ID of the assigner of the assigned grade.
    /// </summary>
    /// <param name="id">ID of the assigned grade.</param>
    /// <returns>
    /// A task that represent the asynchronous operation and the result
    /// of the operation.
    /// </returns>
    Task<GetAssignerIdResponse> GetAssignerIdAsync(int id);
}
