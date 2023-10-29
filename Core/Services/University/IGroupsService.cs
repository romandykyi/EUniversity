﻿using EUniversity.Core.Dtos.University;
using EUniversity.Core.Models.University;

namespace EUniversity.Core.Services.University;

public interface IGroupsService :
    ICrudService<Group, int, GroupPreviewDto, GroupViewDto, GroupCreateDto, GroupCreateDto>
{
    /// <summary>
    /// Adds a student to a group based on the information provided in the
    /// <see cref="StudentGroupDto" /> asynchronously.
    /// </summary>
    /// <param name="dto">The DTO containing data for adding the student to the group.</param>
    /// <returns>
    /// A task that represents the asynchronous operation.
    Task AddStudent(StudentGroupDto dto);

    /// <summary>
    /// Removes a student from a group based on the information provided in the
    /// <see cref="StudentGroupDto" /> asynchronously.
    /// </summary>
    /// <param name="dto">The DTO containing data for removing the student from the group.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. If the student was
    /// successfully removed from the group, it returns <see langword="true" />.
    /// If the student could not be removed from the group, then <see langword="false" />
    /// is returned.
    /// </returns>
    Task<bool> RemoveStudent(StudentGroupDto dto);
}
