using EUniversity.Core.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace EUniversity.IntegrationTests.Services;

/// <summary>
/// An abstract class that implements integration tests for <see cref="IAssigningService{TAssigningEntity, TId1, TId2}" />.
/// </summary>
/// <typeparam name="TService">A type of the <typeparamref name="TService"/> service that is being tested.</typeparam>
/// <typeparam name="TAssigningEntity">A type of entity that configures a many-to-many relationship.</typeparam>
/// <typeparam name="TId1">A type of the ID of the first entity.</typeparam>
/// <typeparam name="TId2">A type of the ID of the second entity.</typeparam>
public abstract class AssigningServiceTests<TService, TAssigningEntity, TId1, TId2> : ServicesTest
    where TService : IAssigningService<TAssigningEntity, TId1, TId2>
    where TAssigningEntity : class
    where TId1 : IEquatable<TId1>
    where TId2 : IEquatable<TId2>
{
    protected TService Service { get; private set; }

    [SetUp]
    public virtual void SetUpService()
    {
        Service = ServiceScope.ServiceProvider.GetService<TService>()!;
    }

    /// <summary>
    /// When implemented, should return an ID of a new existing first entity.
    /// </summary>
    /// <returns>
    /// An ID of a new existing first entity.
    /// </returns>
    protected abstract Task<TId1> GetIdOfExistingEntity1Async();

    /// <summary>
    /// When implemented, should return an ID of a new existing second entity.
    /// </summary>
    /// <returns>
    /// An ID of a new existing second entity.
    /// </returns>
    protected abstract Task<TId2> GetIdOfExistingEntity2Async();

    /// <summary>
    /// When implemented, should return a test assigning entity which can be added to the DB. 
    /// </summary>
    /// <returns>
    /// A test assigning entity which can be added to the DB.
    /// </returns>
    protected abstract TAssigningEntity GetTestAssigningEntity(TId1 id1, TId2 id2);

    /// <summary>
    /// Check if exactly one <typeparamref name="TAssigningEntity"/> that
    /// assignes the first entity to the second exists at the DB.
    /// </summary>
    /// <param name="id1">ID of the first assignable entity.</param>
    /// <param name="id2">ID of the second assignable entity.</param>
    /// <returns>
    /// <see langword="true" /> if exactly one <typeparamref name="TAssigningEntity"/> that
    /// assignes the first entity to the second exists at the DB; 
    /// or <see langword="false"/> otherwise.
    /// </returns>
    protected virtual async Task<bool> CheckAssigningEntityExistenceAsync(TId1 id1, TId2 id2)
    {
        return await DbContext.Set<TAssigningEntity>()
            .SingleOrDefaultAsync(Service.AssigningEntityPredicate(id1, id2)) != null;
    }

    /// <summary>
    /// Creates a test assigning entity using an 
    /// abstract method GetTestAssigningEntity and adds it to the DB.
    /// </summary>
    /// <param name="id1">ID of the first entity.</param>
    /// <param name="id2">ID of the first entity.</param>
    /// <returns>
    /// A test assigning entity that was added to the DB.
    /// </returns>
    protected virtual async Task<TAssigningEntity> CreateTestAssigningEntityAsync(TId1 id1, TId2 id2)
    {
        TAssigningEntity entity = GetTestAssigningEntity(id1, id2);
        DbContext.Add(entity);
        await DbContext.SaveChangesAsync();
        return entity;
    }

    [Test]
    public virtual async Task Assign_EntitiesAreNotAssigned_AssignsAndReturnsTrue()
    {
        // Arrange
        TId1 id1 = await GetIdOfExistingEntity1Async();
        TId2 id2 = await GetIdOfExistingEntity2Async();

        // Act
        bool result = await Service.AssignAsync(id1, id2);

        // Assert that returns true
        Assert.That(result, Is.True);
        // Assert that assigns
        bool exists = await CheckAssigningEntityExistenceAsync(id1, id2);
        Assert.That(exists, "Entities expected to be assigned");
    }

    [Test]
    public virtual async Task Assign_EntitiesAreAlreadyAssigned_ReturnsFalse()
    {
        // Arrange
        TId1 id1 = await GetIdOfExistingEntity1Async();
        TId2 id2 = await GetIdOfExistingEntity2Async();
        await CreateTestAssigningEntityAsync(id1, id2);

        // Act
        bool result = await Service.AssignAsync(id1, id2);

        // Assert
        Assert.That(result, Is.False);
    }

    [Test]
    public virtual async Task Unassign_EnitiesAreAssigned_UnassignsAndReturnsTrue()
    {
        // Arrange
        TId1 id1 = await GetIdOfExistingEntity1Async();
        TId2 id2 = await GetIdOfExistingEntity2Async();
        await CreateTestAssigningEntityAsync(id1, id2);

        // Act
        bool result = await Service.UnassignAsync(id1, id2);

        // Assert that returns true
        Assert.That(result, Is.True);
        // Assert that unassigns
        bool exists = await CheckAssigningEntityExistenceAsync(id1, id2);
        Assert.That(exists, Is.False, "Entities were expected to be unassigned");
    }

    [Test]
    public virtual async Task RemoveStudent_StudentIsNotInGroup_ReturnsFalse()
    {
        // Arrange
        TId1 id1 = await GetIdOfExistingEntity1Async();
        TId2 id2 = await GetIdOfExistingEntity2Async();

        // Act
        bool result = await Service.UnassignAsync(id1, id2);

        // Assert
        Assert.That(result, Is.False);
    }
}
