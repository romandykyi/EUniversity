namespace EUniversity.Infrastructure.Filters;

/// <summary>
/// Represents properties for filtering semesters.
/// </summary>
/// <param name="MaxDateFrom">Minimum start date of the semester to filter by.</param>
/// <param name="MinDateTo">Maximum start date of the semester to filter by.</param>
public record SemestersFilterProperties(DateTimeOffset? MaxDateFrom = null, 
    DateTimeOffset? MinDateTo = null);
