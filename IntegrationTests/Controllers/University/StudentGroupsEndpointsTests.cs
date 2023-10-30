using EUniversity.Core.Dtos.University;
using EUniversity.Core.Models;
using EUniversity.Core.Models.University;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using System.Net;

namespace EUniversity.IntegrationTests.Controllers.University;
public class StudentGroupsEndpointsTests : ControllersTest
{
    public const int TestGroupId = 5;
    public const int NonExistentGroupId = 1;
    public const string TestStudentId = "STUDENT-ID";

    public readonly string AddStudentRoute = $"api/groups/{TestGroupId}/students";
    public readonly string DeleteStudentRoute =
        $"api/groups/{TestGroupId}/students/{TestStudentId}";

    public readonly StudentGroupCreateDto TestDto = new(TestStudentId);

    [SetUp]
    public void SetUp()
    {
        SetUpValidationMocks();
    }

    [Test]
    public async Task AddStudent_UnauthenticatedUser_Returns401Unauthorized()
    {
        // Arrange
        using var client = CreateUnauthorizedClient();
        WebApplicationFactory.GroupsServiceMock
            .AddStudentAsync(Arg.Any<string>(), Arg.Any<int>())
            .Throws<InvalidOperationException>();

        // Act
        var result = await client.PostAsJsonAsync(AddStudentRoute, TestDto);

        // Assert
        Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
    }

    [Test]
    public async Task AddStudent_StudentRole_Returns403Forbidden()
    {
        // Arrange
        using var client = CreateStudentClient();
        WebApplicationFactory.GroupsServiceMock
            .AddStudentAsync(Arg.Any<string>(), Arg.Any<int>())
            .Throws<InvalidOperationException>();

        // Act
        var result = await client.PostAsJsonAsync(AddStudentRoute, TestDto);

        // Assert
        Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.Forbidden));
    }

    [Test]
    public async Task AddStudent_GroupDoesNotExist_Returns404NotFound()
    {
        // Arrange
        using var client = CreateAdministratorClient();
        WebApplicationFactory.GroupsServiceMock
            .AddStudentAsync(Arg.Any<string>(), Arg.Any<int>())
            .Throws<InvalidOperationException>();
        WebApplicationFactory.ExistenceCheckerMock
            .ExistsAsync<Group, int>(TestGroupId)
            .Returns(false);

        // Act
        var result = await client.PostAsJsonAsync(AddStudentRoute, TestDto);

        // Assert
        Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
    }

    [Test]
    public async Task AddStudent_InvalidInput_Returns400BadRequest()
    {
        // Arrange
        using var client = CreateAdministratorClient();
        WebApplicationFactory.GroupsServiceMock
            .AddStudentAsync(Arg.Any<string>(), Arg.Any<int>())
            .Throws<InvalidOperationException>();
        StudentGroupCreateDto dto = new(string.Empty);

        // Act
        var result = await client.PostAsJsonAsync(AddStudentRoute, dto);

        // Assert
        Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
    }

    [Test]
    public async Task AddStudent_StudentIsNotInGroup_Returns201Created()
    {
        // Arrange
        using var client = CreateAdministratorClient();
        WebApplicationFactory.GroupsServiceMock
            .AddStudentAsync(Arg.Any<string>(), Arg.Any<int>())
            .Returns(true);

        // Act
        var result = await client.PostAsJsonAsync(AddStudentRoute, TestDto);

        // Assert
        await WebApplicationFactory.GroupsServiceMock
            .Received(1)
            .AddStudentAsync(TestStudentId, TestGroupId);
        Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.Created));
    }

    [Test]
    public async Task AddStudent_StudentIsAlreadyInGroup_SucceedsWithout201Created()
    {
        // Arrange
        using var client = CreateAdministratorClient();
        WebApplicationFactory.GroupsServiceMock
            .AddStudentAsync(Arg.Any<string>(), Arg.Any<int>())
            .Returns(false);

        // Act
        var result = await client.PostAsJsonAsync(AddStudentRoute, TestDto);

        // Assert
        await WebApplicationFactory.GroupsServiceMock
            .Received(1)
            .AddStudentAsync(TestStudentId, TestGroupId);
        result.EnsureSuccessStatusCode();
        Assert.That(result.StatusCode, Is.Not.EqualTo(HttpStatusCode.Created));
    }

    [Test]
    public async Task RemoveStudent_UnauthenticatedUser_Returns401Unauthorized()
    {
        // Arrange
        using var client = CreateUnauthorizedClient();
        WebApplicationFactory.GroupsServiceMock
            .RemoveStudentAsync(Arg.Any<string>(), Arg.Any<int>())
            .Throws<InvalidOperationException>();

        // Act
        var result = await client.DeleteAsync(DeleteStudentRoute);

        // Assert
        Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
    }

    [Test]
    public async Task RemoveStudent_StudentRole_Returns403Forbidden()
    {
        // Arrange
        using var client = CreateStudentClient();
        WebApplicationFactory.GroupsServiceMock
            .RemoveStudentAsync(Arg.Any<string>(), Arg.Any<int>())
            .Throws<InvalidOperationException>();

        // Act
        var result = await client.DeleteAsync(DeleteStudentRoute);

        // Assert
        Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.Forbidden));
    }

    [Test]
    public async Task RemoveStudent_GroupDoesNotExist_Returns404NotFound()
    {
        // Arrange
        using var client = CreateAdministratorClient();
        WebApplicationFactory.GroupsServiceMock
            .RemoveStudentAsync(Arg.Any<string>(), Arg.Any<int>())
            .Throws<InvalidOperationException>();
        WebApplicationFactory.ExistenceCheckerMock
            .ExistsAsync<Group, int>(TestGroupId)
            .Returns(false);

        // Act
        var result = await client.DeleteAsync(DeleteStudentRoute);

        // Assert
        Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
    }

    [Test]
    public async Task RemoveStudent_StudentDoesNotExist_Returns404NotFound()
    {
        // Arrange
        using var client = CreateAdministratorClient();
        WebApplicationFactory.GroupsServiceMock
            .RemoveStudentAsync(Arg.Any<string>(), Arg.Any<int>())
            .Throws<InvalidOperationException>();
        WebApplicationFactory.ExistenceCheckerMock
            .ExistsAsync<ApplicationUser, string>(TestStudentId)
            .Returns(false);

        // Act
        var result = await client.DeleteAsync(DeleteStudentRoute);

        // Assert
        Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
    }

    [Test]
    public async Task RemoveStudent_StudentIsNotInGroup_Fails()
    {
        // Arrange
        using var client = CreateAdministratorClient();
        WebApplicationFactory.GroupsServiceMock
            .RemoveStudentAsync(Arg.Any<string>(), Arg.Any<int>())
            .Returns(false);

        // Act
        var result = await client.DeleteAsync(DeleteStudentRoute);

        // Assert
        await WebApplicationFactory.GroupsServiceMock
            .Received(1)
            .RemoveStudentAsync(TestStudentId, TestGroupId);
        Assert.That(result.IsSuccessStatusCode, Is.False);
    }

    [Test]
    public async Task RemoveStudent_StudentIsInGroup_Succeeds()
    {
        // Arrange
        using var client = CreateAdministratorClient();
        WebApplicationFactory.GroupsServiceMock
            .RemoveStudentAsync(Arg.Any<string>(), Arg.Any<int>())
            .Returns(true);

        // Act
        var result = await client.DeleteAsync(DeleteStudentRoute);

        // Assert
        await WebApplicationFactory.GroupsServiceMock
            .Received(1)
            .RemoveStudentAsync(TestStudentId, TestGroupId);
        result.EnsureSuccessStatusCode();
    }
}
