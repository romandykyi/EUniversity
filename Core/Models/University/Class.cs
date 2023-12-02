using System.ComponentModel.DataAnnotations.Schema;

namespace EUniversity.Core.Models.University;

/// <summary>
/// Represents a class schedule entity, 
/// which contains date, duration, group and optional substitute teacher.
/// </summary>
public class Class : IEntity<int>, IHasCreationDate, IHasUpdateDate
{
    [Key]
    public int Id { get; set; }

    /// <summary>
    /// Foreign key of the classroom associated with this class.
    /// </summary>
    [ForeignKey(nameof(Classroom))]
    public int ClassroomId { get; set; }
    /// <summary>
    /// Foreign key of the group associated with this class.
    /// </summary>
    [ForeignKey(nameof(Group))]
    public int GroupId { get; set; }
    /// <summary>
    /// Foreign key of the substitute teacher associated with this class(can be null).
    /// </summary>
    [ForeignKey(nameof(SubstituteTeacher))]
    public string? SubstituteTeacherId { get; set; }

    /// <summary>
    /// Date when class starts.
    /// </summary>
    public DateTimeOffset StartDate { get; set; }

    /// <summary>
    /// Duration of this class in ticks.
    /// </summary>
    public long DurationTicks { get; set; }
    /// <summary>
    /// Duration of this class.
    /// </summary>
    public TimeSpan Duration
    {
        get => new(DurationTicks);
        set => DurationTicks = value.Ticks;
    }

    /// <summary>
    /// Date when the class was created.
    /// </summary>
    public DateTimeOffset CreationDate { get; set; }
    /// <summary>
    /// Date when the class was last updated.
    /// </summary>
    public DateTimeOffset UpdateDate { get; set; }

    /// <summary>
    /// Navigation property for the classroom associated with this class.
    /// </summary>
    public Classroom? Classroom { get; set; }
    /// <summary>
    /// Navigation property for the group associated with this class.
    /// </summary>
    public Group? Group { get; set; }
    /// <summary>
    /// Navigation property for the substitute associated with this class(can be null).
    /// </summary>
    public ApplicationUser? SubstituteTeacher { get; set; }
}
