namespace EUniversity.Core.Models.University;

/// <summary>
/// Represents a classroom entity.
/// </summary>
public class Classroom : IEntity<int>, IHasName, IHasCreationDate, IHasUpdateDate
{
    public const int MaxNameLength = 50;

    [Key]
    public int Id { get; set; }
    /// <summary>
    /// Name of the classroom.
    /// </summary>
    [StringLength(MaxNameLength)]
    public string Name { get; set; } = null!;

    /// <summary>
    /// Date when the classroom was created.
    /// </summary>
    public DateTimeOffset CreationDate { get; set; }
    /// <summary>
    /// Date when the classroom was last updated.
    /// </summary>
    public DateTimeOffset UpdateDate { get; set; }
}
