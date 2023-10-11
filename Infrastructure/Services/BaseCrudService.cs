using EUniversity.Core.Models;
using EUniversity.Core.Services;
using EUniversity.Infrastructure.Data;

namespace EUniversity.Infrastructure.Services
{
    /// <summary>
    /// Abstract class for performing CRUD operations on entities.
    /// </summary>
    /// <remarks>
    /// This class uses <see cref="ApplicationDbContext" /> and these mappings:
    /// * TEntity -> TDetailsDto
    /// * TCreateDto -> TEntity
    /// * TUpdateDto -> TEntity
    /// </remarks>
    /// <typeparam name="TEntity">The entity type that implements <see cref="IEntity{TId}" />.</typeparam>
    /// <typeparam name="TId">The type of the entity's unique identifier.</typeparam>
    /// <typeparam name="TDetailsDto">The DTO type for entity details.</typeparam>
    /// <typeparam name="TCreateDto">The DTO type for creating a new entity.</typeparam>
    /// <typeparam name="TUpdateDto">The DTO type for updating an existing entity.</typeparam>
    public abstract class BaseCrudService<TEntity, TId, TDetailsDto, TCreateDto, TUpdateDto> :
        ICrudService<TEntity, TId, TDetailsDto, TCreateDto, TUpdateDto>
        where TEntity : class, IEntity<TId>
        where TId : IEquatable<TId>
    {
        protected ApplicationDbContext DbContext { get; init; }
        protected DbSet<TEntity> Entities => DbContext.Set<TEntity>();

        protected BaseCrudService(ApplicationDbContext dbContext)
        {
            DbContext = dbContext;
        }

        /// <inheritdoc />
        public async Task<TId> CreateAsync(TCreateDto dto)
        {
            TEntity entity = dto.Adapt<TEntity>();
            DbContext.Set<TEntity>().Add(entity);
            await DbContext.SaveChangesAsync();

            return entity.Id;
        }

        /// <inheritdoc />
        public async Task<bool> DeleteAsync(TId id)
        {
            // Find entity by its ID
            var entity = await Entities
                .FirstOrDefaultAsync(e => e.Id.Equals(id));
            // Entity does not exist - return false
            if (entity == null) return false;

            // Remove entity
            DbContext.Remove(entity);
            await DbContext.SaveChangesAsync();

            return true;
        }

        /// <inheritdoc />
        public async Task<TDetailsDto?> GetByIdAsync(TId id)
        {
            var entity = await Entities.AsNoTracking()
                .FirstOrDefaultAsync(e => e.Id.Equals(id));

            return entity.Adapt<TDetailsDto>();
        }

        /// <inheritdoc />
        public async Task<bool> UpdateAsync(TId id, TUpdateDto dto)
        {
            // Try to find an existing entity
            var entity = await DbContext.Set<TEntity>()
                .FirstOrDefaultAsync(e => e.Id.Equals(id));
            if (entity == null) return false;

            // Update the entity
            dto.Adapt(entity);
            DbContext.Update(entity);
            await DbContext.SaveChangesAsync();

            return true;
        }
    }
}
