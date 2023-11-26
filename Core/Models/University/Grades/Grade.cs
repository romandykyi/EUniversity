namespace EUniversity.Core.Models.University.Grades;

/// <summary>
/// Represents an available grade entity that can be assigned to students.
/// </summary>
public class Grade : IEntity<int>, IHasName, IHasCreationDate, IHasUpdateDate
{
    public const int MaxNameLength = 50;

    [Key]
    public int Id { get; set; }
    /// <summary>
    /// Displayed name of the grade.
    /// </summary>
    [StringLength(MaxNameLength)]
    public string Name { get; set; } = null!;
    /// <summary>
    /// Numeric value of the grade.
    /// </summary>
    public int Score { get; set; }

    /// <summary>
    /// Date when the grade was created.
    /// </summary>
    public DateTimeOffset CreationDate { get; set; }
    /// <summary>
    /// Date when the grade was last updated.
    /// </summary>
    public DateTimeOffset UpdateDate { get; set; }

    /// <summary>
    /// Navigation property: 
    /// Collection of <see cref="CourseGrades" />s related to this grade.
    /// </summary>
    public IEnumerable<CourseGrade> CourseGrades { get; set; } = null!;
}
