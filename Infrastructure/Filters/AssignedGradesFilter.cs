using EUniversity.Core.Filters;
using EUniversity.Core.Models.University.Grades;

namespace EUniversity.Infrastructure.Filters;

/// <summary>
/// Filter for assigned grades.
/// </summary>
public class AssignedGradesFilter : IFilter<AssignedGrade>
{
    /// <summary>
    /// Student ID to filter by.
    /// </summary>
    public string? StudentId { get; }
    /// <summary>
    /// Group ID to filter by.
    /// </summary>
    public int? GroupId { get; }
    /// <summary>
    /// Additional filter properties that can be passed as query parameters.
    /// </summary>
    public AssignedGradesFilterProperties Properties { get; }

    public AssignedGradesFilter(AssignedGradesFilterProperties properties,
        string? studentId = null, int? groupId = null)
    {
        Properties = properties;
        StudentId = studentId;
        GroupId = groupId;
    }

    /// <summary>
    /// Applies the assigned grades filter and sorts results by assignation date.
    /// </summary>
    /// <param name="query">The query that needs to be filtered.</param>
    /// <returns>
    /// Filtered query with assigned grades which satisfy filter properties and sorted
    /// by assignation date.
    /// </returns>
    public IQueryable<AssignedGrade> Apply(IQueryable<AssignedGrade> query)
    {
        if (Properties.AssignerId != null)
        {
            query = query.Where(g => g.AssignerId == Properties.AssignerId);
        }
        if (Properties.ReassignerId != null)
        {
            query = query.Where(g => g.ReassignerId == Properties.ReassignerId);
        }
        if (StudentId != null)
        {
            query = query.Where(g => g.StudentId == StudentId);
        }
        if (GroupId != null)
        {
            query = query.Where(g => g.GroupId == GroupId);
        }
        if (Properties.ActivityTypeId != null)
        {
            query = Properties.ActivityTypeId != 0 ?
                query.Where(g => g.ActivityTypeId == Properties.ActivityTypeId) :
                query.Where(g => g.ActivityTypeId == null);
        }
        if (Properties.GradeId != null)
        {
            query = query.Where(g => g.GradeId == Properties.GradeId);
        }
        return query.OrderByDescending(g => g.CreationDate);
    }
}
