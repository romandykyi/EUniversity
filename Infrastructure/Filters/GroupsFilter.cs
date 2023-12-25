using EUniversity.Core.Models.University;

namespace EUniversity.Infrastructure.Filters;

/// <summary>
/// Filter for groups.
/// </summary>
public class GroupsFilter : DefaultFilter<Group>
{
    /// <summary>
    /// Properties used for filtering.
    /// </summary>
    public GroupsFilterProperties Properties { get; }

    public GroupsFilter(GroupsFilterProperties properties, 
        string name, DefaultFilterSortingMode sortingMode = DefaultFilterSortingMode.Default) : 
        base(name, sortingMode)
    {
        Properties = properties;
    }

    /// <inheritdoc />
    public override IQueryable<Group> Apply(IQueryable<Group> query)
    {
        if (Properties.TeacherId != null)
        {
            query = query.Where(g => g.TeacherId == Properties.TeacherId);
        }
        if (Properties.SemesterId != null)
        {
            int? semesterId = Properties.SemesterId != 0 ? Properties.SemesterId : null;
            query = query.Where(g => g.Course.SemesterId == semesterId);
        }
        if (Properties.CourseId != null)
        {
            query = query.Where(g => g.CourseId == Properties.CourseId);
        }
        return base.Apply(query);
    }
}
