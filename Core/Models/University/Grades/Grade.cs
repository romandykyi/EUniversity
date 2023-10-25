namespace EUniversity.Core.Models.University.Grades;

public class Grade : IEntity<int>
{
    public const int MaxNameLength = 50;

    [Key]
    public int Id { get; set; }
    [StringLength(MaxNameLength)]
    public string Name { get; set; } = null!;
    public int Score { get; set; }

    public IEnumerable<CourseGrade> CourseGrades { get; set; } = null!;
}
