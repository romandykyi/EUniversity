using EUniversity.Core.Models;
using EUniversity.Core.Services;
using EUniversity.Infrastructure.Data;

namespace EUniversity.Infrastructure.Services;

/// <summary>
/// Class that can check the existence of entity in the database.
/// </summary>
public class EntityExistenceChecker : IEntityExistenceChecker
{
    private readonly ApplicationDbContext _dbContext;

    public EntityExistenceChecker(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <inheritdoc />
    public async Task<bool> ExistsAsync<TEntity, TId>(TId id)
        where TEntity : class, IEntity<TId>
        where TId : IEquatable<TId>
    {
        return await _dbContext.Set<TEntity>()
            .AnyAsync(e => e.Id.Equals(id));
    }
}
