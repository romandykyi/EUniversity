using EUniversity.Core.Services;
using EUniversity.Infrastructure.Data;
using System.Linq.Expressions;

namespace EUniversity.Infrastructure.Services;

/// <summary>
/// An abstract class that provides methods for 
/// assigning/unassigning entities in a many-to-many relationship.
/// </summary>
/// <typeparam name="TAssigningEntity">A type of entity that configures a many-to-many relationship.</typeparam>
/// <typeparam name="TId1">A type of the ID of the first entity.</typeparam>
/// <typeparam name="TId2">A type of the ID of the second entity.</typeparam>
public abstract class AssigningService<TAssigningEntity, TId1, TId2> : IAssigningService<TAssigningEntity, TId1, TId2>
    where TAssigningEntity : class
    where TId1 : IEquatable<TId1>
    where TId2 : IEquatable<TId2>
{
    protected ApplicationDbContext DbContext { get; init; }
    protected DbSet<TAssigningEntity> AssigningEntities => DbContext.Set<TAssigningEntity>();

    public AssigningService(ApplicationDbContext dbContext)
    {
        DbContext = dbContext;
    }

    /// <summary>
    /// When implemented, creates a new assigning entity instance which
    /// assigns an entity with <paramref name="id1"/> to an entity with <paramref name="id2"/>
    /// </summary>
    /// <param name="id1">ID of the first entity.</param>
    /// <param name="id2">ID of the second entity.</param>
    /// <returns>
    /// A new assigning entity which assigns the first entity to the second.
    /// </returns>
    protected abstract TAssigningEntity CreateAssigningEntity(TId1 id1, TId2 id2);

    /// <summary>
    /// When implemented, gets a predicate that can be used for finding
    /// an instance of assigning entity in the database.
    /// </summary>
    /// <param name="id1">ID of the first entity.</param>
    /// <param name="id2">ID of the second entity.</param>
    /// <returns>
    /// A predicate that can be used for finding
    /// an instance of assigning entity in the database.
    /// </returns>
    public abstract Expression<Func<TAssigningEntity, bool>> AssigningEntityPredicate(TId1 id1, TId2 id2);

    /// <inheritdoc />
    public virtual async Task<bool> AssignAsync(TId1 entity1Id, TId2 entity2Id)
    {
        // Check if entities are already assigned
        bool entitiesAreAssigned = await AssigningEntities
            .AnyAsync(AssigningEntityPredicate(entity1Id, entity2Id));
        if (entitiesAreAssigned)
        {
            return false;
        }

        // Assign entities
        TAssigningEntity assigningEntity = CreateAssigningEntity(entity1Id, entity2Id);
        AssigningEntities.Add(assigningEntity);
        await DbContext.SaveChangesAsync();

        return true;
    }

    /// <inheritdoc />
    public virtual async Task<bool> UnassignAsync(TId1 entity1Id, TId2 entity2Id)
    {
        // Check if entities are assigned
        var assigningEntity = await AssigningEntities
        .FirstOrDefaultAsync(
             AssigningEntityPredicate(entity1Id, entity2Id)
            );
        // Entities are not assigned - return false
        if (assigningEntity == null) return false;

        // Unassign entities
        DbContext.Remove(assigningEntity);
        await DbContext.SaveChangesAsync();

        return true;
    }
}
