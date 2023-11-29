using EUniversity.Core.Filters;
using EUniversity.Core.Models;

namespace EUniversity.Infrastructure.Filters;

/// <summary>
/// Filter for student enrollments.
/// </summary>
public class StudentEnrollmentsFilter : IFilter<IStudentEnrollment>
{
    public StudentEnrollmentsFilterProperties Properties { get; set; }

    /// <summary>
    /// Initializes a new instance of the class with the specified properties.
    /// </summary>
    /// <param name="properties">Properties of filtering.</param>
    public StudentEnrollmentsFilter(StudentEnrollmentsFilterProperties properties)
    {
        Properties = properties;
    }

    public IQueryable<IStudentEnrollment> Apply(IQueryable<IStudentEnrollment> query)
    {
        throw new NotImplementedException();
    }
}
