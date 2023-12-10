namespace EUniversity.Core.Models.University;

public class ActivityType : IEntity<int>, IHasName, IHasCreationDate, IHasUpdateDate
{
    public const int MaxNameLength = 100;

    [Key]
    public int Id { get; set; }
    /// <summary>
    /// Name of the activity type.
    /// </summary>
    [StringLength(MaxNameLength)]
    public string Name { get; set; } = null!;

    /// <summary>
    /// Date when the activity type was created.
    /// </summary>
    public DateTimeOffset CreationDate { get; set; }
    /// <summary>
    /// Date when the activity was last updated.
    /// </summary>
    public DateTimeOffset UpdateDate { get; set; }
}
