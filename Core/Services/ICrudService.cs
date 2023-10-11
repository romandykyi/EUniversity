using EUniversity.Core.Models;

namespace EUniversity.Core.Services
{
    /// <summary>
    /// Represents an interface for performing CRUD operations on entities.
    /// </summary>
    /// <typeparam name="TEntity">The entity type that implements <see cref="IEntity{TId}" />.</typeparam>
    /// <typeparam name="TId">The type of the entity's unique identifier.</typeparam>
    /// <typeparam name="TDetailsDto">The DTO type for entity details.</typeparam>
    /// <typeparam name="TCreateDto">The DTO type for creating a new entity.</typeparam>
    /// <typeparam name="TUpdateDto">The DTO type for updating an existing entity.</typeparam>
    public interface ICrudService<TEntity, TId, TDetailsDto, TCreateDto, TUpdateDto>
        where TEntity : class, IEntity<TId>
        where TId : IEquatable<TId>
    {
        /// <summary>
        /// Retrieves an entity's details by its unique identifier asynchronously.
        /// </summary>
        /// <param name="id">The unique identifier of the entity to retrieve.</param>
        /// <returns>
        /// A task that represents the asynchronous operation, containing 
        /// the entity details or <see langword="null" /> if the entity does not exist.
        /// </returns>
        Task<TDetailsDto?> GetByIdAsync(TId id);

        /// <summary>
        /// Creates a new entity based on the information provided in the
        /// <see cref="TCreateDto" /> asynchronously.
        /// </summary>
        /// <param name="dto">The DTO containing data for creating the entity.</param>
        /// <returns>
        /// A task that represents the asynchronous operation, containing
        /// the identifier of the newly created entity.
        /// </returns>
        Task<TId> CreateAsync(TCreateDto dto);

        /// <summary>
        /// Updates an existing entity identified by its unique identifier asynchronously.
        /// </summary>
        /// <param name="id">The unique identifier of the entity to update.</param>
        /// <param name="dto">The DTO containing data for updating the entity.</param>
        /// <returns>
        /// A task that represents the asynchronous operation. If the 
        /// entity is updated successfully, it returns <see langword="true" />.
        /// If the entity with the specified identifier is not found, 
        /// it returns <see langword="false" />.
        /// </returns>
        Task<bool> UpdateAsync(TId id, TUpdateDto dto);

        /// <summary>
        /// Deletes an entity identified by its unique identifier asynchronously.
        /// </summary>
        /// <param name="id">The unique identifier of the entity to delete.</param>
        /// <returns>
        /// A task that represents the asynchronous operation. If the entity 
        /// is deleted successfully, it returns <see langword="true" />.
        /// If the entity with the specified identifier is not found, 
        /// it returns <see langword="false" />.
        /// </returns>
        Task<bool> DeleteAsync(TId id);
    }
}
