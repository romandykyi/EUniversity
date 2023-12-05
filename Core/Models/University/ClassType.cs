namespace EUniversity.Core.Models.University;

/// <summary>
/// Represents a class type(e.g. lecture).
/// </summary>
public class ClassType : IEntity<int>, IHasName, IHasCreationDate, IHasUpdateDate
{
    public const int MaxNameLength = 100;

    [Key]
    public int Id { get; set; }
    /// <summary>
    /// Name of the class type.
    /// </summary>
    [StringLength(MaxNameLength)]
    public string Name { get; set; } = null!;

    /// <summary>
    /// Date when the class type was created.
    /// </summary>
    public DateTimeOffset CreationDate { get; set; }
    /// <summary>
    /// Date when the class type was last updated.
    /// </summary>
    public DateTimeOffset UpdateDate { get; set; }
}
