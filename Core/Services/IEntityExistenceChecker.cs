using EUniversity.Core.Models;

namespace EUniversity.Core.Services;

/// <summary>
/// Represents an interface for checking the existence of entities.
/// </summary>
public interface IEntityExistenceChecker
{
    /// <summary>
    /// Asynchronously checks whether an entity of a specified 
    /// type exists by its unique identifier.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity to check for existence.</typeparam>
    /// <typeparam name="TId">The type of the unique identifier for the entity.</typeparam>
    /// <param name="id">The unique identifier of the entity.</param>
    /// <returns>
    /// A Task that represents the asynchronous operation, 
    /// returning a boolean value indicating whether the entity exists.
    /// </returns>
    Task<bool> ExistsAsync<TEntity, TId>(TId id)
        where TEntity : class, IEntity<TId>
        where TId : IEquatable<TId>;
}
