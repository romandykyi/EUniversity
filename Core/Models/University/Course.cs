using System.ComponentModel.DataAnnotations.Schema;

namespace EUniversity.Core.Models.University;

/// <summary>
/// Represents a course entity.
/// </summary>
public class Course : IEntity<int>, IHasName
{
    public const int MaxNameLength = 200;
    public const int MaxDescriptionLength = 1000;

    [Key]
    public int Id { get; set; }
    /// <summary>
    /// Foreign key of the semester associated with this course(may be null).
    /// </summary>
    [ForeignKey(nameof(Semester))]
    public int? SemesterId { get; set; }
    /// <summary>
    /// Name of the course.
    /// </summary>
    [StringLength(MaxNameLength)]
    public string Name { get; set; } = null!;
    /// <summary>
    /// Optional description of the course.
    /// </summary>
    [StringLength(MaxDescriptionLength)]
    public string? Description { get; set; }

    /// <summary>
    /// Navigation property of the semester associated with this course(may be null).
    /// </summary>
    public Semester? Semester { get; set; }
}
