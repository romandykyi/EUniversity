namespace EUniversity.Core.Models;

/// <summary>
/// Represents an interface for entities with a unique identifier.
/// </summary>
/// <typeparam name="T">The type of the unique identifier (e.g., int, string).</typeparam>
public interface IEntity<T> where T : IEquatable<T>
{
    /// <summary>
    /// The unique identifier(primary key) of the entity.
    /// </summary>
    public T Id { get; set; }
}
