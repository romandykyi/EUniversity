using System.Linq.Expressions;

namespace EUniversity.Core.Services;

/// <summary>
/// Represents an interface for assigning/unassigning entities in a many-to-many relationship.
/// </summary>
/// <typeparam name="TAssigningEntity">A type of entity that configures a many-to-many relationship.</typeparam>
/// <typeparam name="TId1">A type of the ID of the first entity.</typeparam>
/// <typeparam name="TId2">A type of the ID of the second entity.</typeparam>
public interface IAssigningService<TAssigningEntity, TId1, TId2>
    where TAssigningEntity : class
    where TId1 : IEquatable<TId1>
    where TId2 : IEquatable<TId2>
{
    /// <summary>
    /// Gets a predicate that can be used for finding
    /// an instance of assigning entity in the database.
    /// </summary>
    /// <param name="id1">ID of the first entity.</param>
    /// <param name="id2">ID of the second entity.</param>
    /// <returns>
    /// A predicate that can be used for finding
    /// an instance of assigning entity in the database.
    /// </returns>
    public Expression<Func<TAssigningEntity, bool>> AssigningEntityPredicate(TId1 id1, TId2 id2);

    /// <summary>
    /// Adds the first entity to the second based on their IDs.
    /// </summary>
    /// <param name="entity1Id">ID of the first entity.</param>
    /// <param name="entity2Id">ID of the second entity.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. If the first entity was
    /// successfully assigned to the second, it returns <see langword="true" />.
    /// If the first entity was already assigned to the second, then <see langword="false" />
    /// is returned.
    /// </returns>
    public Task<bool> AssignAsync(TId1 entity1Id, TId2 entity2Id);

    /// <summary>
    /// Unasigns the first entity from the second based on their IDs.
    /// </summary>
    /// <param name="entity1Id">ID of the first entity.</param>
    /// <param name="entity2Id">ID of the second entity.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. If the first entity was
    /// successfully unassigned from the second, it returns <see langword="true" />.
    /// If the first entity was not assigned to the second, then <see langword="false" />
    /// is returned.
    /// </returns>
    public Task<bool> UnassignAsync(TId1 entity1Id, TId2 entity2Id);
}
