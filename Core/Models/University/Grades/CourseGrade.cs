using System.ComponentModel.DataAnnotations.Schema;

namespace EUniversity.Core.Models.University.Grades;

/// <summary>
/// Represents a course grade entity, which is the final 
/// grade assigned by the teacher to the student.
/// </summary>
public class CourseGrade : AssignedGradeBase<int>
{
    /// <summary>
    /// Foreign key of the course associated with this grade.
    /// </summary>
    [ForeignKey(nameof(Course))]
    public int CourseId { get; set; }
    /// <summary>
    /// Navigation property to the course associated with this grade.
    /// </summary>
    public Course Course { get; set; } = null!;
}
