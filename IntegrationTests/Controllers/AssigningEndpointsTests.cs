using EUniversity.Core.Dtos.University;
using EUniversity.Core.Models;
using EUniversity.Core.Services;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using System.Net;

namespace EUniversity.IntegrationTests.Controllers;

/// <summary>
/// An abstract class that implements integration tests for endpoints
/// that use <see cref="IAssigningService{TAssigningEntity, TId1, TId2}" />
/// </summary>
/// <typeparam name="TService">A type of the <typeparamref name="TService"/> service that is used in tested endpoints.</typeparam>
/// <typeparam name="TAssigningEntity">A type of entity that configures a many-to-many relationship.</typeparam>
/// <typeparam name="TEntity1">A type of the first entity.</typeparam>
/// <typeparam name="TId1">A type of the ID of the first entity.</typeparam>
/// <typeparam name="TEntity2">A type of the second entity.</typeparam>
/// <typeparam name="TId2">A type of the ID of the second entity.</typeparam>
public abstract class AssigningEndpointsTests<TService, TAssigningEntity, TEntity1, TId1, TEntity2, TId2> : ControllersTest
    where TService : class, IAssigningService<TAssigningEntity, TId1, TId2>
    where TAssigningEntity : class
    where TEntity1 : class, IEntity<TId1>
    where TId1 : IEquatable<TId1>
    where TEntity2 : class, IEntity<TId2>
    where TId2 : IEquatable<TId2>
{
    /// <summary>
    /// Mock of the assigning service. Should be initialized in <see cref="SetUpService"/>.
    /// </summary>
    protected TService ServiceMock { get; set; } = default!;

    /// <summary>
    /// Sets up a <see cref="ServiceMock"/> used for testing.
    /// </summary>
    [SetUp]
    public abstract void SetUpService();

    [SetUp]
    public virtual void SetUp()
    {
        SetUpValidationMocks();
    }

    /// <summary>
    /// Test ID of the first entity.
    /// </summary>
    public abstract TId1 TestId1 { get; }
    /// <summary>
    /// Test ID of the second entity.
    /// </summary>
    public abstract TId2 TestId2 { get; }

    /// <summary>
    /// Route used for assigning an entity.
    /// E.g.: "api/entities/[TestId1]/subentities/".
    /// </summary>
    public abstract string AssignRoute { get; }
    /// <summary>
    /// Route used for unassigning an entity.
    /// E.g.: "api/entities/[TestId1]/subentities/[TestId2]".
    /// </summary>
    public abstract string UnassignRoute { get; }

    /// <summary>
    /// When implemented, should return a DTO which is passed to the assigning route.
    /// </summary>
    /// <returns>
    /// A DTO which is passed to the assigning route.
    /// </returns>
    protected abstract object GetAssignDto();

    [Test]
    public async Task Assign_UnauthenticatedUser_Returns401Unauthorized()
    {
        // Arrange
        using var client = CreateUnauthorizedClient();
        ServiceMock
            .AssignAsync(Arg.Any<TId1>(), Arg.Any<TId2>())
            .Throws<InvalidOperationException>();

        // Act
        var result = await client.PostAsJsonAsync(AssignRoute, GetAssignDto());

        // Assert
        Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
    }

    [Test]
    public async Task Assign_StudentRole_Returns403Forbidden()
    {
        // Arrange
        using var client = CreateStudentClient();
        ServiceMock
            .AssignAsync(Arg.Any<TId1>(), Arg.Any<TId2>())
            .Throws<InvalidOperationException>();

        // Act
        var result = await client.PostAsJsonAsync(AssignRoute, GetAssignDto());

        // Assert
        Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.Forbidden));
    }

    [Test]
    public async Task Assign_Entity1DoesNotExist_Returns404NotFound()
    {
        // Arrange
        using var client = CreateAdministratorClient();
        ServiceMock
            .AssignAsync(Arg.Any<TId1>(), Arg.Any<TId2>())
            .Throws<InvalidOperationException>();
        WebApplicationFactory.ExistenceCheckerMock
            .ExistsAsync<TEntity1, TId1>(TestId1)
            .Returns(false);

        // Act
        var result = await client.PostAsJsonAsync(AssignRoute, GetAssignDto());

        // Assert
        Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
    }

    [Test]
    public async Task Assign_InvalidInput_Returns400BadRequest()
    {
        // Arrange
        using var client = CreateAdministratorClient();
        ServiceMock
            .AssignAsync(Arg.Any<TId1>(), Arg.Any<TId2>())
            .Throws<InvalidOperationException>();
        AssignStudentDto dto = new(string.Empty);

        // Act
        var result = await client.PostAsJsonAsync(AssignRoute, dto);

        // Assert
        Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
    }

    [Test]
    public async Task Assign_EntitiesAreNotAssigned_Returns201Created()
    {
        // Arrange
        using var client = CreateAdministratorClient();
        ServiceMock
            .AssignAsync(Arg.Any<TId1>(), Arg.Any<TId2>())
            .Returns(true);

        // Act
        var result = await client.PostAsJsonAsync(AssignRoute, GetAssignDto());

        // Assert
        await ServiceMock
            .Received(1)
            .AssignAsync(TestId1, TestId2);
        Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.Created));
    }

    [Test]
    public async Task Assign_EntitiesAreAlreadyAssigned_SucceedsWithout201Created()
    {
        // Arrange
        using var client = CreateAdministratorClient();
        ServiceMock
            .AssignAsync(Arg.Any<TId1>(), Arg.Any<TId2>())
            .Returns(false);

        // Act
        var result = await client.PostAsJsonAsync(AssignRoute, GetAssignDto());

        // Assert
        await ServiceMock
            .Received(1)
            .AssignAsync(TestId1, TestId2);
        result.EnsureSuccessStatusCode();
        Assert.That(result.StatusCode, Is.Not.EqualTo(HttpStatusCode.Created));
    }

    [Test]
    public async Task Unassign_UnauthenticatedUser_Returns401Unauthorized()
    {
        // Arrange
        using var client = CreateUnauthorizedClient();
        ServiceMock
            .UnassignAsync(Arg.Any<TId1>(), Arg.Any<TId2>())
            .Throws<InvalidOperationException>();

        // Act
        var result = await client.DeleteAsync(UnassignRoute);

        // Assert
        Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
    }

    [Test]
    public async Task Unassign_StudentRole_Returns403Forbidden()
    {
        // Arrange
        using var client = CreateStudentClient();
        ServiceMock
            .UnassignAsync(Arg.Any<TId1>(), Arg.Any<TId2>())
            .Throws<InvalidOperationException>();

        // Act
        var result = await client.DeleteAsync(UnassignRoute);

        // Assert
        Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.Forbidden));
    }

    [Test]
    public async Task Unassign_Entity1DoesNotExist_Returns404NotFound()
    {
        // Arrange
        using var client = CreateAdministratorClient();
        ServiceMock
            .UnassignAsync(Arg.Any<TId1>(), Arg.Any<TId2>())
            .Throws<InvalidOperationException>();
        WebApplicationFactory.ExistenceCheckerMock
            .ExistsAsync<TEntity1, TId1>(TestId1)
            .Returns(false);

        // Act
        var result = await client.DeleteAsync(UnassignRoute);

        // Assert
        Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
    }

    [Test]
    public async Task Unassign_Entity2DoesNotExist_Returns404NotFound()
    {
        // Arrange
        using var client = CreateAdministratorClient();
        ServiceMock
            .UnassignAsync(Arg.Any<TId1>(), Arg.Any<TId2>())
            .Throws<InvalidOperationException>();
        WebApplicationFactory.ExistenceCheckerMock
            .ExistsAsync<TEntity2, TId2>(TestId2)
            .Returns(false);

        // Act
        var result = await client.DeleteAsync(UnassignRoute);

        // Assert
        Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
    }

    [Test]
    public async Task Unassign_EntitiesAreUnassigned_Fails()
    {
        // Arrange
        using var client = CreateAdministratorClient();
        ServiceMock
            .UnassignAsync(Arg.Any<TId1>(), Arg.Any<TId2>())
            .Returns(false);

        // Act
        var result = await client.DeleteAsync(UnassignRoute);

        // Assert
        await ServiceMock
            .Received(1)
            .UnassignAsync(TestId1, TestId2);
        Assert.That(result.IsSuccessStatusCode, Is.False);
    }

    [Test]
    public async Task Unassign_EntitiesAreAssigned_Succeeds()
    {
        // Arrange
        using var client = CreateAdministratorClient();
        ServiceMock
            .UnassignAsync(Arg.Any<TId1>(), Arg.Any<TId2>())
            .Returns(true);

        // Act
        var result = await client.DeleteAsync(UnassignRoute);

        // Assert
        await ServiceMock
            .Received(1)
            .UnassignAsync(TestId1, TestId2);
        result.EnsureSuccessStatusCode();
    }
}
