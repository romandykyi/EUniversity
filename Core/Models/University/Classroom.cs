namespace EUniversity.Core.Models.University;

/// <summary>
/// Represents a classroom entity.
/// </summary>
public class Classroom : IEntity<int>
{
    public const int MaxNameLength = 50;

    [Key]
    public int Id { get; set; }
    /// <summary>
    /// Name of the classroom.
    /// </summary>
    [StringLength(MaxNameLength)]
    public string Name { get; set; } = null!;
}
