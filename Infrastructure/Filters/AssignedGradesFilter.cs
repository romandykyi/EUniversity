using EUniversity.Core.Filters;
using EUniversity.Core.Models.University.Grades;

namespace EUniversity.Infrastructure.Filters;

/// <summary>
/// Filter for assigned grades.
/// </summary>
public class AssignedGradesFilter : IFilter<AssignedGrade>
{
    public AssignedGradesFilterProperties Properties { get; }

    public AssignedGradesFilter(AssignedGradesFilterProperties properties)
    {
        Properties = properties;
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
        if (Properties.StudentId != null)
        {
            query = query.Where(g => g.StudentId == Properties.StudentId);
        }
        if (Properties.GroupId != null)
        {
            query = query.Where(g => g.GroupId == Properties.GroupId);
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
