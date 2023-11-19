﻿namespace EUniversity.Core.Models.University;

/// <summary>
/// An entity that represents a semester.
/// </summary>
public class Semester : IEntity<int>, IHasName
{
    public const int MaxNameLength = 100;

    [Key]
    public int Id { get; set; }
    /// <summary>
    /// Name of the semester.
    /// </summary>
    [StringLength(MaxNameLength)]
    public string Name { get; set; } = null!;
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