using System.ComponentModel.DataAnnotations.Schema;

namespace EUniversity.Core.Models.University;

/// <summary>
/// Represents the entity used for configuring many-to-many relationship
/// between students and groups.
/// </summary>
public class StudentGroup : IEntity<int>, IStudentEnrollment
{
    [Key]
    public int Id { get; set; }

    /// <summary>
    /// Date when student was added to the group.
    /// </summary>
    public DateTimeOffset EnrollmentDate { get; set; }

    /// <summary>
    /// Foreign key of the associated student.
    /// </summary>
    [ForeignKey(nameof(Student))]
    public string StudentId { get; set; } = null!;
    /// <summary>
    /// Foreign key of the associated group.
    /// </summary>
    [ForeignKey(nameof(Group))]
    public int GroupId { get; set; }

    /// <summary>
    /// Navigation property for the student associated with this group.
    /// </summary>
    public ApplicationUser Student { get; set; } = null!;
    /// <summary>
    /// Navigation property for the group that the student is part of.
    /// </summary>
    public Group Group { get; set; } = null!;
}
