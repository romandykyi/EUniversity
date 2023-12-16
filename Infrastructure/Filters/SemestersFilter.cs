using EUniversity.Core.Models.University;

namespace EUniversity.Infrastructure.Filters;

/// <summary>
/// Filter for classes.
/// </summary>
public class SemestersFilter : DefaultFilter<Semester>
{
    /// <summary>
    /// Properties of filtering.
    /// </summary>
    public SemestersFilterProperties Properties { get; }

    public SemestersFilter(SemestersFilterProperties properties, string name,
        DefaultFilterSortingMode sortingMode = DefaultFilterSortingMode.Default) :
        base(name, sortingMode)
    {
        Properties = properties;
    }

    /// <inheritdoc />
    public override IQueryable<Semester> Apply(IQueryable<Semester> query)
    {
        // Apply semesters filter
        if (Properties.MaxDateFrom != null)
        {
            query = query.Where(s => s.DateFrom <= Properties.MaxDateFrom);
        }
        if (Properties.MinDateTo != null)
        {
            query = query.Where(s => s.DateTo >= Properties.MinDateTo);
        }
        // Apply default filter
        return base.Apply(query);
    }
}
