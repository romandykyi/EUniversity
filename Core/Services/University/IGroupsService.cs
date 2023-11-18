using EUniversity.Core.Dtos.University;
using EUniversity.Core.Models.University;

namespace EUniversity.Core.Services.University;

public interface IGroupsService :
    ICrudService<Group, int, GroupPreviewDto, GroupViewDto, GroupCreateDto, GroupCreateDto>
{
    /// <summary>
    /// Adds a student to a group based on the information.
    /// </summary>
    /// <param name="studentId">ID of the student to add to the group.</param>
    /// <param name="groupId">ID of the group to which the user will be added.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. If the student was
    /// successfully added to the group, it returns <see langword="true" />.
    /// If the student was already part of the group., then <see langword="false" />
    /// is returned.
    /// </returns>
    Task<bool> AddStudentAsync(string studentId, int groupId);

    /// <summary>
    /// Removes a student from a group based on the information.
    /// </summary>
    /// <param name="studentId">ID of the student to remove from the group.</param>
    /// <param name="groupId">ID of the group from which the user will be removed.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. If the student was
    /// successfully removed from the group, it returns <see langword="true" />.
    /// If the student was not part of the group., then <see langword="false" />
    /// is returned.
    /// </returns>
    Task<bool> RemoveStudentAsync(string studentId, int groupId);
}
