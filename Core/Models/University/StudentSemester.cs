using System.ComponentModel.DataAnnotations.Schema;

namespace EUniversity.Core.Models.University;

/// <summary>
/// Represents the entity used for configuring many-to-many relationship
/// between students and semesters.
/// </summary>
public class StudentSemester : IEntity<int>
{
    [Key]
    public int Id { get; set; }

    /// <summary>
    /// Date when student was added to the semester.
    /// </summary>
    public DateTimeOffset EnrolmentDate { get; set; }

    /// <summary>
    /// Foreign key of the associated student.
    /// </summary>
    [ForeignKey(nameof(Student))]
    public string StudentId { get; set; } = null!;
    /// <summary>
    /// Foreign key of the associated semester.
    /// </summary>
    [ForeignKey(nameof(Semester))]
    public int SemesterId { get; set; }

    /// <summary>
    /// Navigation property for the student associated with this semester.
    /// </summary>
    public ApplicationUser Student { get; set; } = null!;
    /// <summary>
    /// Navigation property for the semester that the student is part of.
    /// </summary>
    public Semester Semester { get; set; } = null!;
}
