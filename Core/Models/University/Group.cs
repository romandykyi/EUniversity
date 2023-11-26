using System.ComponentModel.DataAnnotations.Schema;

namespace EUniversity.Core.Models.University;

/// <summary>
/// Represents a group entity, 
/// which contains students with its own teacher and course.
/// </summary>
public class Group : IEntity<int>, IHasName, IHasCreationDate, IHasUpdateDate
{
    public const int MaxNameLength = 50;

    [Key]
    public int Id { get; set; }
    /// <summary>
    /// Name of the group.
    /// </summary>
    [StringLength(MaxNameLength)]
    public string Name { get; set; } = null!;
    /// <summary>
    /// Foreign key of the course associated with this group.
    /// </summary>
    [ForeignKey(nameof(Course))]
    public int CourseId { get; set; }
    /// <summary>
    /// Foreign key of the teacher associated with this group.
    /// </summary>
    [ForeignKey(nameof(Teacher))]
    public string? TeacherId { get; set; }

    /// <summary>
    /// Date when the group was created.
    /// </summary>
    public DateTimeOffset CreationDate { get; set; }
    /// <summary>
    /// Date when the group was last updated.
    /// </summary>
    public DateTimeOffset UpdateDate { get; set; }

    /// <summary>
    /// Navigation property to the course associated with this group.
    /// </summary>
    public Course Course { get; set; } = null!;
    /// <summary>
    /// Navigation property to the teacher associated with this group.
    /// </summary>
    public ApplicationUser? Teacher { get; set; }
    /// <summary>
    /// Navigation property to the students which are part of this group.
    /// </summary>
    public ICollection<ApplicationUser> Students { get; set; } = null!;
}
