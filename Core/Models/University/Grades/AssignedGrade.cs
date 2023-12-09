using System.ComponentModel.DataAnnotations.Schema;

namespace EUniversity.Core.Models.University.Grades;

/// <summary>
/// A base class for assigned grades.
/// </summary>
public abstract class AssignedGrade : IEntity<int>, IHasCreationDate, IHasUpdateDate
{
    public const int MaxNotesLength = 500;

    [Key]
    public int Id { get; set; } = default!;
    /// <summary>
    /// Foreign key of the assigned grade.
    /// </summary>
    [ForeignKey(nameof(Grade))]
    public int GradeId { get; set; }
    /// <summary>
    /// Foreign key of the group associated with this grade.
    /// </summary>
    [ForeignKey(nameof(Group))]
    public int GroupId { get; set; }
    /// <summary>
    /// Foreign key of the user, who assigned the grade.
    /// </summary>
    [ForeignKey(nameof(Assigner))]
    public string? AssignerId { get; set; }
    /// <summary>
    /// Foreign key of the user, who reassigned the grade.
    /// </summary>
    [ForeignKey(nameof(Reassigner))]
    public string? ReassignerId { get; set; }
    /// <summary>
    /// Foreign key of the student, who was graded.
    /// </summary>
    [ForeignKey(nameof(Student))]
    public string? StudentId { get; set; }
    /// <summary>
    /// Date, when the grade was assigned.
    /// </summary>
    public DateTimeOffset CreationDate { get; set; }
    /// <summary>
    /// Date when the grade was last updated.
    /// </summary>
    public DateTimeOffset UpdateDate { get; set; }
    /// <summary>
    /// Optional notes related to the grade.
    /// </summary>
    public string? Notes { get; set; }

    /// <summary>
    /// Navigation property for the assigned grade.
    /// </summary>
    public Grade Grade { get; set; } = null!;
    /// <summary>
    /// Navigation property for the group associated with this grade.
    /// </summary>
    public Group Group { get; set; } = null!;
    /// <summary>
    /// Navigation property for the user, who assigned the grade.
    /// </summary>
    public ApplicationUser? Assigner { get; set; }
    /// <summary>
    /// Navigation property for the user, who reassigned the grade.
    /// </summary>
    public ApplicationUser? Reassigner { get; set; }
    /// <summary>
    /// Navigation property for the student, who was graded.
    /// </summary>
    public ApplicationUser? Student { get; set; }
}
