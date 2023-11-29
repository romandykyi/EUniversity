namespace EUniversity.Core.Models;

/// <summary>
/// An interface that defines an entity with enrollment date and student information.
/// </summary>
public interface IStudentEnrollment
{
    public DateTimeOffset EnrollmentDate { get; }
    public ApplicationUser? Student { get; }
}
