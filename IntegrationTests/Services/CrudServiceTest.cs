using EUniversity.Core.Models;
using EUniversity.Core.Services;

namespace EUniversity.IntegrationTests.Services
{
    /// <summary>
    /// Base tests for CRUD services.
    /// </summary>
    /// <typeparam name="TService">The CRUD service type</typeparam>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TId"></typeparam>
    /// <typeparam name="TDetailsDto"></typeparam>
    /// <typeparam name="TCreateDto"></typeparam>
    /// <typeparam name="TUpdateDto"></typeparam>
    public class CrudServiceTest<TService, TEntity, TId, TDetailsDto, TCreateDto, TUpdateDto> : ServicesTest
        where TService : ICrudService<TEntity, TId, TDetailsDto, TCreateDto, TUpdateDto>
        where TEntity : class, IEntity<TId>
        where TId : IEquatable<TId>
    {
    }
}
