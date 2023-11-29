using EUniversity.Core.Filters;
using EUniversity.Core.Models;
using System.Linq.Expressions;

namespace EUniversity.Infrastructure.Filters;

/// <summary>
/// Filter for student enrollments.
/// </summary>
public class StudentEnrollmentsFilter<T> : IFilter<T>
    where T : IStudentEnrollment
{
    private readonly Expression<Func<T, string>> FullNameKeySelector =
        e => e.Student!.FirstName + ' ' + (e.Student.MiddleName != null ? e.Student.MiddleName + ' ' : "") + e.Student.LastName;

    public StudentEnrollmentsFilterProperties Properties { get; set; }

    /// <summary>
    /// Initializes a new instance of the class with the specified properties.
    /// </summary>
    /// <param name="properties">Properties of filtering.</param>
    public StudentEnrollmentsFilter(StudentEnrollmentsFilterProperties properties)
    {
        Properties = properties;
    }

    public IQueryable<T> Apply(IQueryable<T> query)
    {
        if (Properties.FullName != null)
        {
            query = query.Where(
                e => (e.Student!.FirstName + ' ' +
                (e.Student.MiddleName != null ? e.Student!.MiddleName + ' ' : "") + e.Student.LastName)
                .Contains(Properties.FullName)
                );
        }
        switch (Properties.SortingMode)
        {
            case StudentEnrollmentsSortingMode.FullName:
                query = query.OrderBy(FullNameKeySelector);
                break;
            case StudentEnrollmentsSortingMode.FullNameDescending:
                query = query.OrderByDescending(FullNameKeySelector);
                break;
            case StudentEnrollmentsSortingMode.Newest:
                query = query.OrderByDescending(e => e.EnrollmentDate);
                break;
            case StudentEnrollmentsSortingMode.Oldest:
                query = query.OrderBy(e => e.EnrollmentDate);
                break;
        }

        return query;
    }
}
