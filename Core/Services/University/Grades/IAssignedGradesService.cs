﻿using EUniversity.Core.Dtos.University.Grades;
using EUniversity.Core.Filters;
using EUniversity.Core.Models.University.Grades;
using EUniversity.Core.Pagination;

namespace EUniversity.Core.Services.University.Grades;

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
    /// <typeparam name="TViewDto">A type of returned DTOs.</typeparam>
    /// <returns>
    /// A task that represents the asynchronous operation, containing 
    /// the page with entities previews.
    /// </returns>
    Task<Page<TViewDto>> GetPageAsync<TViewDto>(PaginationProperties properties, IFilter<AssignedGrade>? filter = null);

    /// <summary>
    /// Assignes a grade to a student based on the information provided in the
    /// <see cref="dto" /> asynchronously.
    /// </summary>
    /// <param name="dto">The DTO containing data for assigning the grade.</param>
    /// <param name="AssignerId">ID of the user who assigns the grade.</param>
    /// <returns>
    /// A task that represents the asynchronous operation, containing
    /// the newly created entity.
    /// </returns>
    Task<AssignedGrade> AssignAsync(AssignedGradeCreateDto dto, string AssignerId);

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
}
