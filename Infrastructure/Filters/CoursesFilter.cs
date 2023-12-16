using EUniversity.Core.Models.University;

namespace EUniversity.Infrastructure.Filters;

/// <summary>
/// Filter for classes.
/// </summary>
public class CoursesFilter : DefaultFilter<Course>
{
    /// <summary>
    /// Properties of filtering.
    /// </summary>
    public CoursesFilterProperties Properties { get; }

    public CoursesFilter(CoursesFilterProperties properties, string name,
        DefaultFilterSortingMode sortingMode = DefaultFilterSortingMode.Default) :
        base(name, sortingMode)
    {
        Properties = properties;
    }

    /// <inheritdoc />
    public override IQueryable<Course> Apply(IQueryable<Course> query)
    {
        // Apply courses filter
        if (Properties.SemesterId != null)
        {
            int? semesterId = Properties.SemesterId != 0 ? Properties.SemesterId : null;
            query = query.Where(c => c.SemesterId == semesterId);
        }
        // Apply default filter
        return base.Apply(query);
    }
}
