namespace EUniversity.Core.Models;

/// <summary>
/// An interface that defines an entity that has a creation date property.
/// </summary>
public interface IHasCreationDate
{
    /// <summary>
    /// Date when the entity was created.
    /// </summary>
    public DateTimeOffset CreationDate { get; set; }
}
