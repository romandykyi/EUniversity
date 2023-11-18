namespace EUniversity.Core.Models.University;

public class Semester : IEntity<int>
{
    [Key]
    public int Id { get; set; }
    /// <summary>
    /// Date when the semester starts.
    /// </summary>
    public DateTimeOffset DateFrom { get; set; }
    /// <summary>
    /// Date when the semester ends.
    /// </summary>
    public DateTimeOffset DateTo { get; set; }

    /// <summary>
    /// Navigation property to the students which are part of this semester.
    /// </summary>
    public ICollection<ApplicationUser> Students { get; set; } = null!;
    /// <summary>
    /// Navigation property to the student enrollments of this semester.
    /// </summary>
    public ICollection<StudentSemester> StudentEnrollments { get; set; } = null!;
}
