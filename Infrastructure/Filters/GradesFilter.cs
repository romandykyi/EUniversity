using Bogus.DataSets;
using EUniversity.Core.Filters;
using EUniversity.Core.Models;
using EUniversity.Core.Models.University.Grades;

namespace EUniversity.Infrastructure.Filters;

/// <summary>
/// Represents a sorting mode for <see cref="GradesFilter" />.
/// </summary>
public enum GradesSortingMode
{
    /// <summary>
    /// Default sorting mode, grades will not be sorted.
    /// </summary>
    Default = 0,
    /// <summary>
    /// Sort grades by name alphabetically from a to z.
    /// </summary>
    Name = 1,
    /// <summary>
    /// Sort grades by name alphabetically from z to a.
    /// </summary>
    NameDescending = 2,
    /// <summary>
    /// Sort grades by its score.
    /// </summary>
    Score = 3,
    /// <summary>
    /// Sort grades by its score descending.
    /// </summary>
    ScoreDescending = 4
}

/// <summary>
/// Filter for grades.
/// </summary>
public class GradesFilter : IFilter<Grade>
{
    /// <summary>
    /// Properties used for filtering grades.
    /// </summary>
    public GradesFilterProperties Properties { get; set; }

    public GradesFilter(GradesFilterProperties properties)
    {
        Properties = properties;
    }

    /// <inheritdoc />
    public virtual IQueryable<Grade> Apply(IQueryable<Grade> query)
    {
        // Filter by name
        query = query.Where(g => g.Name.Contains(Properties.Name));

        // Apply sort
        switch (Properties.SortingMode)
        {
            case GradesSortingMode.Name:
                query = query.OrderBy(g => g.Name);
                break;
            case GradesSortingMode.NameDescending:
                query = query.OrderByDescending(g => g.Name);
                break;
            case GradesSortingMode.Score:
                query = query.OrderBy(g => g.Score);
                break;
            case GradesSortingMode.ScoreDescending:
                query = query.OrderByDescending(g => g.Score);
                break;
        }

        return query;
    }
}
