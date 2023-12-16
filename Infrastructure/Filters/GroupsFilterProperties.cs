namespace EUniversity.Infrastructure.Filters;

/// <summary>
/// Represents properties for filtering classes.
/// </summary>
/// <param name="TeacherId">Optional ID of the teacher to filter by.</param>
/// <param name="CourseId">Optional ID of the course to filter by.</param>
/// <param name="SemesterId">
/// Optional ID of the semester to filter by.
/// If 0, then groups that are not linked to any semesters will be returned.
/// </param>
public record GroupsFilterProperties(string? TeacherId = null, int? CourseId = null, int? SemesterId = null);
