namespace EUniversity.Infrastructure.Filters;

/// <summary>
/// Represents a sorting mode for <see cref="StudentEnrollmentsFilter" />.
/// </summary>
public enum StudentEnrollmentsSortingMode
{
    /// <summary>
    /// Default sorting mode, enrollments will not be sorted.
    /// </summary>
    Default = 0,
    /// <summary>
    /// Sort enrollments by student's fullname alphabetically from a to z.
    /// </summary>
    FullName = 1,
    /// <summary>
    /// Sort enrollments by student's fullname alphabetically from z to a. 
    /// </summary>
    FullNameDescending = 2,
    /// <summary>
    /// Sort enrollments by date descending.
    /// </summary>
    Newest = 3,
    /// <summary>
    /// Sort enrollments by date ascending.
    /// </summary>
    Oldest = 4
}

/// <summary>
/// Represents properties for filtering student enrollments.
/// </summary>
/// <param name="FullName">Optional full name to filter students by.</param>
/// <param name="SortingMode">
/// Optional sorting mode, <see cref="StudentEnrollmentsSortingMode.FullName"/> by default.
/// </param>
public record StudentEnrollmentsFilterProperties(string? FullName = null, 
    StudentEnrollmentsSortingMode SortingMode = StudentEnrollmentsSortingMode.FullName);
