namespace EUniversity.Infrastructure.Filters;

/// <summary>
/// Represents properties for filtering classes.
/// </summary>
/// <param name="TeacherId">
/// Optional ID of the teacher to filter by. 
/// Search by this property is based on the substitute teacher, if it's present.
/// </param>
/// <param name="GroupId">ID of the group to filter by.</param>
/// <param name="ClassroomId">ID of the classroom to filter by.</param>
/// <param name="ClassTypeId">ID of the class type to filter by.</param>
/// <param name="MinStartDate">Minimum start date of the class to filter by.</param>
/// <param name="MaxStartDate">Maximum start date of the class to filter by.</param>
public record ClassesFilterProperties(string? TeacherId = null,
    int? GroupId = null, int? ClassroomId = null, int? ClassTypeId = null,
    DateTimeOffset? MinStartDate = null, DateTimeOffset? MaxStartDate = null);