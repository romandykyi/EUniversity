namespace EUniversity.Infrastructure.Filters;

/// <summary>
/// Properties for <see cref="AssignedGradesFilter" />
/// </summary>
/// <param name="AssignerId">An optional ID of an assigner to filter by.</param>
/// <param name="ReassignerId">An optional ID of a reassigner to filter by.</param>
/// <param name="StudentId">An optional ID of an assignee student to filter by.</param>
/// <param name="GroupId">An optional ID of a gorup to filter by.</param>
/// <param name="ActivityTypeId">An optional ID of an activity type to filter by. If 0, then grades without activity type will be returned.</param>
/// <param name="GradeId">An optional ID of a grade to filter by.</param>
public record AssignedGradesFilterProperties(
    string? AssignerId = null, string? ReassignerId = null,
    string? StudentId = null, int? GroupId = null,
    int? ActivityTypeId = null, int? GradeId = null);
