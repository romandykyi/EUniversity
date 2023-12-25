namespace EUniversity.Infrastructure.Filters;

/// <summary>
/// Represents properties for filtering grades.
/// </summary>
/// <param name="Name">Optional name to filter by.</param>
/// <param name="SortingMode">Sorting mode for grades.</param>
public record GradesFilterProperties(string Name = "", GradesSortingMode SortingMode = GradesSortingMode.ScoreDescending);
