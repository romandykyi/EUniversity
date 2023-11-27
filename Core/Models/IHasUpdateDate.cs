namespace EUniversity.Core.Models;

/// <summary>
/// An interface that defines an entity that has an update(edit) date property.
/// </summary>
public interface IHasUpdateDate
{
    /// <summary>
    /// Date when the entity was last updated.
    /// </summary>
    public DateTimeOffset UpdateDate { get; set; }
}
