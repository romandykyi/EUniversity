﻿using EUniversity.Core.Filters;
using EUniversity.Core.Models;
using EUniversity.Core.Pagination;
using EUniversity.Core.Services;
using EUniversity.Infrastructure.Data;
using EUniversity.Infrastructure.Pagination;

namespace EUniversity.Infrastructure.Services;

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
/// <typeparam name="TPreviewDto">The DTO type for entity preview in the paged list.</typeparam>
/// <typeparam name="TDetailsDto">The DTO type for entity details.</typeparam>
/// <typeparam name="TCreateDto">The DTO type for creating a new entity.</typeparam>
/// <typeparam name="TUpdateDto">The DTO type for updating an existing entity.</typeparam>
public abstract class BaseCrudService<TEntity, TId, TPreviewDto, TDetailsDto, TCreateDto, TUpdateDto> :
    ICrudService<TEntity, TId, TPreviewDto, TDetailsDto, TCreateDto, TUpdateDto>
    where TEntity : class, IEntity<TId>
    where TId : IEquatable<TId>
{
    protected ApplicationDbContext DbContext { get; init; }
    protected DbSet<TEntity> Entities => DbContext.Set<TEntity>();

    /// <summary>
    /// Query used for obtaining an entity by its ID.
    /// </summary>
    protected virtual IQueryable<TEntity> GetByIdQuery => Entities.AsNoTracking();
    /// <summary>
    /// Query used for obtaining a page with entities.
    /// </summary>
    protected virtual IQueryable<TEntity> GetPageQuery => Entities.AsNoTracking();

    protected BaseCrudService(ApplicationDbContext dbContext)
    {
        DbContext = dbContext;
    }

    /// <inheritdoc />
    public virtual async Task<TEntity> CreateAsync(TCreateDto dto)
    {
        TEntity entity = dto.Adapt<TEntity>();
        // Set a creation date(if possible)
        if (entity is IHasCreationDate entityWithCreationDate)
        {
            entityWithCreationDate.CreationDate = DateTimeOffset.Now;
        }
        // Set an update date(if possible)
        if (entity is IHasUpdateDate entityWithUpdateDate)
        {
            entityWithUpdateDate.UpdateDate = DateTimeOffset.Now;
        }
        DbContext.Set<TEntity>().Add(entity);
        await DbContext.SaveChangesAsync();

        return entity;
    }

    /// <inheritdoc />
    public virtual async Task<bool> DeleteAsync(TId id)
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
    public virtual async Task<Page<TPreviewDto>> GetPageAsync(PaginationProperties properties, IFilter<TEntity>? filter = null)
    {
        var query = GetPageQuery;
        if (filter != null) query = filter.Apply(query);

        return await query.ToPageAsync<TEntity, TPreviewDto>(properties);
    }

    /// <inheritdoc />
    public virtual async Task<TDetailsDto?> GetByIdAsync(TId id)
    {
        var entity = await GetByIdQuery
            .FirstOrDefaultAsync(e => e.Id.Equals(id));

        return entity.Adapt<TDetailsDto>();
    }

    /// <inheritdoc />
    public virtual async Task<bool> UpdateAsync(TId id, TUpdateDto dto)
    {
        // Try to find an existing entity
        var entity = await DbContext.Set<TEntity>()
            .FirstOrDefaultAsync(e => e.Id.Equals(id));
        if (entity == null) return false;

        // Update the entity
        dto.Adapt(entity);
        // Set an update date(if possible)
        if (entity is IHasUpdateDate entityWithUpdateDate)
        {
            entityWithUpdateDate.UpdateDate = DateTimeOffset.Now;
        }
        DbContext.Update(entity);
        await DbContext.SaveChangesAsync();

        return true;
    }
}
