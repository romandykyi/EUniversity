using System.ComponentModel.DataAnnotations.Schema;

namespace EUniversity.Core.Models.University.Grades;

public class CourseGrade : AssignedGradeBase<int>
{
    [ForeignKey(nameof(Course))]
    public int CourseId { get; set; }
    public Course Course { get; set; } = null!;
}
