using System.ComponentModel.DataAnnotations.Schema;

namespace EUniversity.Core.Models.University.Grades;

/// <summary>
/// Represents an abstract base class for assigned grades.
/// </summary>
public abstract class AssignedGradeBase<TId> : IEntity<TId>
    where TId : IEquatable<TId>
{
    public const int MaxNotesLength = 500;

    [Key]
    public TId Id { get; set; } = default!;
    /// <summary>
    /// Foreign key of of the assigned grade.
    /// </summary>
    [ForeignKey(nameof(Grade))]
    public int GradeId { get; set; }
    /// <summary>
    /// Foreign key of the teacher, who assigned the grade.
    /// </summary>
    [ForeignKey(nameof(Teacher))]
    public string? TeacherId { get; set; }
    /// <summary>
    /// Foreign key of the student, who was graded.
    /// </summary>
    [ForeignKey(nameof(Student))]
    public string? StudentId { get; set; }
    /// <summary>
    /// Date, when the grade was assigned.
    /// </summary>
    public DateTimeOffset Date { get; set; }
    /// <summary>
    /// Optional notes related to the grade.
    /// </summary>
    public string? Notes { get; set; }

    /// <summary>
    /// Navigation property for the assigned grade.
    /// </summary>
    public Grade Grade { get; set; } = null!;
    /// <summary>
    /// Navigation property for the teacher, who assigned the grade.
    /// </summary>
    public ApplicationUser? Teacher { get; set; }
    /// <summary>
    /// Navigation property for the student, who was graded.
    /// </summary>
    public ApplicationUser? Student { get; set; }
}
