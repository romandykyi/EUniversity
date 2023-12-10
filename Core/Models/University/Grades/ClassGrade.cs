namespace EUniversity.Core.Models.University.Grades;

/// <summary>
/// A class for assigned class grades.
/// </summary>
public class ClassGrade : AssignedGrade
{
    /// <summary>
    /// Optional foreign key of activity type of the grade.
    /// </summary>
    public int? ActivityTypeId { get; set; }

    /// <summary>
    /// Navigation property for the activity type of the grade.
    /// </summary>
    public ActivityType? ActivityType { get; set; }
}
