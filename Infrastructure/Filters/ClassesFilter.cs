using EUniversity.Core.Filters;
using EUniversity.Core.Models.University;

namespace EUniversity.Infrastructure.Filters;

/// <summary>
/// Filter for classes.
/// </summary>
public class ClassesFilter : IFilter<Class>
{
    public ClassesFilterProperties Properties { get; }

    public ClassesFilter(ClassesFilterProperties properties)
    {
        Properties = properties;
    }

    /// <summary>
    /// Applies the classes filter and sorts results by start date.
    /// </summary>
    /// <param name="query">The query that needs to be filtered.</param>
    /// <returns>
    /// Filtered query that contains classes which satisfy filter properties and sorted
    /// by start date.
    /// </returns>
    public IQueryable<Class> Apply(IQueryable<Class> query)
    {
        if (Properties.ClassTypeId != null)
        {
            query = query.Where(c => c.ClassTypeId == Properties.ClassTypeId);
        }
        if (Properties.ClassroomId != null)
        {
            query = query.Where(c => c.ClassroomId == Properties.ClassroomId);
        }
        if (Properties.GroupId != null)
        {
            query = query.Where(c => c.GroupId == Properties.GroupId);
        }
        if (Properties.MinStartDate != null)
        {
            query = query.Where(c => c.StartDate >= Properties.MinStartDate);
        }
        if (Properties.MaxStartDate != null)
        {
            query = query.Where(c => c.StartDate <= Properties.MaxStartDate);
        }
        if (Properties.StudentId != null)
        {
            query = query.Where(c => 
                c.Group!.Students.Select(s => s.Id).Contains(Properties.StudentId));
        }
        if (Properties.TeacherId != null)
        {
            query = query.Where(c => c.SubstituteTeacherId != null ?
                c.SubstituteTeacherId == Properties.TeacherId :
                c.Group!.TeacherId == Properties.TeacherId
            );
        }
        return query.OrderBy(c => c.StartDate);
    }
}
