using EUniversity.Core.Dtos.University;
using EUniversity.Core.Models.University;

namespace EUniversity.Core.Services.University;

public interface ISemestersService :
    ICrudService<Semester, int, SemesterPreviewDto, SemesterViewDto, SemesterCreateDto, SemesterCreateDto>
{
    /// <summary>
    /// Adds a student to a semester based on the information.
    /// </summary>
    /// <param name="studentId">ID of the student to add to the semester.</param>
    /// <param name="semesterId">ID of the semester to which the user will be added.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. If the student was
    /// successfully added to the semester, it returns <see langword="true" />.
    /// If the student was already part of the semester., then <see langword="false" />
    /// is returned.
    /// </returns>
    Task<bool> AddStudentAsync(string studentId, int semesterId);

    /// <summary>
    /// Removes a student from a semester based on the information.
    /// </summary>
    /// <param name="studentId">ID of the student to remove from the semester.</param>
    /// <param name="semesterId">ID of the semester from which the user will be removed.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. If the student was
    /// successfully removed from the semester, it returns <see langword="true" />.
    /// If the student was not part of the semester., then <see langword="false" />
    /// is returned.
    /// </returns>
    Task<bool> RemoveStudentAsync(string studentId, int semesterId);
}
