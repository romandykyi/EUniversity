using EUniversity.Core.Dtos.Auth;
using EUniversity.Core.Dtos.University;
using EUniversity.Core.Dtos.Users;
using EUniversity.Core.Filters;
using EUniversity.Core.Models;
using EUniversity.Core.Models.University;
using EUniversity.Core.Pagination;
using EUniversity.Core.Policy;
using EUniversity.Core.Services.Auth;
using EUniversity.Infrastructure.Filters;
using Microsoft.AspNetCore.Identity;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using NSubstitute.ReceivedExtensions;
using NSubstitute.ReturnsExtensions;
using System.Net;

namespace EUniversity.IntegrationTests.Controllers;

public class UsersControllerTests : ControllersTest
{
    public readonly RegisterDto RegisterUser1 = new("example-email1@email.com", "Test1", "Test1");
    public readonly RegisterDto RegisterUser2 = new("example-email2@email.com", "Test2", "Test2");
    public const int SampleRegisterUsersCount = 2;
    private RegisterUsersDto SampleRegisterUsers => new(
        new RegisterDto[SampleRegisterUsersCount]
        {
            RegisterUser1,
            RegisterUser2
        });

    public UserPreviewDto[] TestUsers =
    {
        new("1", "mail1@example.com", "user1", "First1", "Last1", null),
        new("2", "mail2@example.com", "user2", "First2", "Last2", "Middle2")
    };

    public UsersFilterProperties TestFilterProperties = new("Joe Doe", "joedoe777", "mail@example.com", UsersSortingMode.UserName);
    public const string TestFilterQuery = "fullName=Joe%20Doe&userName=joedoe777&email=mail@example.com&sortingMode=username";

    public static readonly string[] GetMethods =
    {
        "/api/users",
        "/api/users/deleted",
        "/api/users/students",
        "/api/users/teachers"
    };
    public static readonly string[] RolesGetMethods =
    {
        "/api/users/students",
        "/api/users/teachers"
    };
    public static readonly string[] RegisterMethods =
    {
        "/api/users/students",
        "/api/users/teachers"
    };
    public static readonly (string, string)[] RegisterMethodsWithRoles =
    {
        (RolesGetMethods[0], Roles.Student),
        (RolesGetMethods[1], Roles.Teacher)
    };

    private Page<UserPreviewDto> GetTestPage(PaginationProperties paginationProperties)
    {
        return new(TestUsers, paginationProperties, TestUsers.Length);
    }

    [SetUp]
    public void SetUp()
    {
        WebApplicationFactory.UsersServiceMock
            .GetUsersInRoleAsync(Arg.Any<string>(), Arg.Any<PaginationProperties>(), Arg.Any<IFilter<ApplicationUser>>())
            .Returns(x => GetTestPage((PaginationProperties)x[1]));
        WebApplicationFactory.UsersServiceMock
            .GetAllUsersAsync(Arg.Any<PaginationProperties>(), Arg.Any<IFilter<ApplicationUser>>(), Arg.Any<bool>())
            .Returns(x => GetTestPage((PaginationProperties)x[0]));
    }

    [Test]
    [TestCaseSource(nameof(GetMethods))]
    public async Task GetMethods_AdministratorRole_SucceedAndReturnValidType(string method)
    {
        // Arrange
        using var client = CreateAdministratorClient();

        // Act
        var result = await client.GetAsync(method);

        // Assert
        result.EnsureSuccessStatusCode();
        var users = await result.Content.ReadFromJsonAsync<Page<UserPreviewDto>>();
        Assert.That(users, Is.Not.Null);
    }

    [Test]
    [TestCaseSource(nameof(GetMethods))]
    public async Task GetMethods_PaginationQueryParams_SucceedAndApplyPagination(string method)
    {
        // Arrange
        using var client = CreateAdministratorClient();
        const int page = 2, pageSize = 25;

        // Act
        var result = await client.GetAsync($"{method}?page={page}&pageSize={pageSize}");

        // Assert
        result.EnsureSuccessStatusCode();
        var usersPage = await result.Content.ReadFromJsonAsync<Page<UserPreviewDto>>();
        Assert.That(usersPage, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(usersPage.PageNumber, Is.EqualTo(page));
            Assert.That(usersPage.PageSize, Is.EqualTo(pageSize));
            Assert.That(usersPage.Items.Count(), Is.LessThanOrEqualTo(pageSize));
        });
    }

    [Test]
    public async Task GetAllUsers_FilterQueryParams_SucceedsAndAppliesFilter()
    {
        // Arrange
        using var client = CreateAdministratorClient();

        // Act
        var result = await client.GetAsync($"/api/users?{TestFilterQuery}");

        // Assert
        result.EnsureSuccessStatusCode();
        var usersPage = await result.Content.ReadFromJsonAsync<Page<UserPreviewDto>>();
        Assert.That(usersPage, Is.Not.Null);
        await WebApplicationFactory.UsersServiceMock
            .Received()
            .GetAllUsersAsync(Arg.Any<PaginationProperties>(),
            Arg.Is<UsersFilter>(f => f.Properties == TestFilterProperties));
    }

    [Test]
    [TestCaseSource(nameof(RolesGetMethods))]
    public async Task GetUsersInRoleMethods_FilterQueryParams_SucceedAndApplyFilter(string method)
    {
        // Arrange
        using var client = CreateAdministratorClient();

        // Act
        var result = await client.GetAsync($"{method}?{TestFilterQuery}");

        // Assert
        result.EnsureSuccessStatusCode();
        var usersPage = await result.Content.ReadFromJsonAsync<Page<UserPreviewDto>>();
        Assert.That(usersPage, Is.Not.Null);
        await WebApplicationFactory.UsersServiceMock
            .Received()
            .GetUsersInRoleAsync(Arg.Any<string>(), Arg.Any<PaginationProperties>(),
            Arg.Is<UsersFilter>(f => f.Properties == TestFilterProperties));
    }

    [Test]
    [TestCaseSource(nameof(GetMethods))]
    public async Task GetMethods_UnauthenticatedUser_Return401Unauthorized(string method)
    {
        // Arrange
        using var client = CreateUnauthorizedClient();

        // Act
        var result = await client.GetAsync(method);

        // Assert
        Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
    }

    [Test]
    public async Task GetUsers_NoAdministratorRole_Return403Forbidden()
    {
        // Arrange
        using var client = CreateStudentClient();

        // Act
        var result = await client.GetAsync("api/users");

        // Assert
        Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.Forbidden));
    }

    [Test]
    public async Task GetStudents_NoAdministratorOrTeacherRole_Return403Forbidden()
    {
        // Arrange
        using var client = CreateStudentClient();

        // Act
        var result = await client.GetAsync("api/users/students");

        // Assert
        Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.Forbidden));
    }

    [Test]
    [TestCaseSource(nameof(RegisterMethods))]
    public async Task RegisterMethods_MalformedInput_Return400BadRequest(string method)
    {
        // Arrange
        using var client = CreateAdministratorClient();
        RegisterUsersDto users = new(Enumerable.Empty<RegisterDto>());

        // Act
        var result = await client.PostAsJsonAsync(method, users);

        // Assert
        Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
    }

    [Test]
    [TestCaseSource(nameof(RegisterMethods))]
    public async Task RegisterMethods_UnauthenticatedUser_Return401Unauthorized(string method)
    {
        // Arrange
        using var client = CreateUnauthorizedClient();

        // Act
        var result = await client.PostAsJsonAsync(method, SampleRegisterUsers);

        // Assert
        Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
    }

    [Test]
    [TestCaseSource(nameof(RegisterMethods))]
    public async Task RegisterMethods_NoAdministratorRole_Return403Forbidden(string method)
    {
        // Arrange
        using var client = CreateStudentClient();

        // Act
        var result = await client.PostAsJsonAsync(method, SampleRegisterUsers);

        // Assert
        Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.Forbidden));
    }

    [Test]
    [TestCaseSource(nameof(RegisterMethodsWithRoles))]
    public async Task RegisterMethods_ValidInput_RegisterValidRoles(
        (string, string) method)
    {
        // Arrange
        using var client = CreateAdministratorClient();
        WebApplicationFactory.AuthServiceMock
            .RegisterManyAsync(Arg.Any<IEnumerable<RegisterDto>>(), Arg.Any<string>())
            .Returns(AsyncEnumerable.Repeat<RegisterResult>(
                new(IdentityResult.Success), SampleRegisterUsersCount));

        // Act
        var result = await client.PostAsJsonAsync(method.Item1, SampleRegisterUsers);

        // Assert
        result.EnsureSuccessStatusCode();
        WebApplicationFactory.AuthServiceMock
            .Received(1)
            .RegisterManyAsync(Arg.Any<IEnumerable<RegisterDto>>(), method.Item2);
    }

    [Test]
    [TestCaseSource(nameof(RegisterMethods))]
    public async Task RegisterMethods_ValidInput_ReturnValidType(string method)
    {
        // Arrange
        using var client = CreateAdministratorClient();
        WebApplicationFactory.AuthServiceMock
            .RegisterAsync(Arg.Any<RegisterDto>(), Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string[]>())
            .Returns(Task.FromResult(new RegisterResult(IdentityResult.Success)));

        // Act
        var result = await client.PostAsJsonAsync(method, SampleRegisterUsers);

        // Assert
        result.EnsureSuccessStatusCode();
        var users = await result.Content.ReadFromJsonAsync<IEnumerable<CreatedUserDto>>();
        Assert.That(users, Is.Not.Null);
    }

    [Test]
    public async Task GetStudentGroups_ValidInput_SucceedsAndAppliesFilter()
    {
        // Arrange
        using var client = CreateAdministratorClient();
        string studentId = Guid.NewGuid().ToString();
        WebApplicationFactory.UsersServiceMock
            .GetGroupsOfStudentAsync(Arg.Any<string>(), Arg.Any<PaginationProperties>(),
            Arg.Any<IFilter<Group>>())
            .Returns(Task.FromResult(new Page<GroupPreviewDto>()));
        PaginationProperties paginationProperties = new(1, 25);
        GroupsFilterProperties filterProperties = new(TeacherId: "test-teacher-id", SemesterId: 0);

        // Act
        var result = await client.GetAsync($"api/users/students/{studentId}/groups?page={paginationProperties.Page}&pageSize={paginationProperties.PageSize}&teacherId={filterProperties.TeacherId}&semesterId={filterProperties.SemesterId}");

        // Assert
        result.EnsureSuccessStatusCode();
        var groups = await result.Content.ReadFromJsonAsync<Page<GroupPreviewDto>>();
        Assert.That(groups, Is.Not.Null);
        await WebApplicationFactory.UsersServiceMock
            .Received()
            .GetGroupsOfStudentAsync(studentId, paginationProperties,
            Arg.Is<GroupsFilter>(f => f.Properties == filterProperties));
    }

    [Test]
    public async Task GetStudentGroups_StudentAccessesAnotherStudentEnrollments_Returns403Forbidden()
    {
        // Arrange
        using var client = CreateStudentClient("student-id");
        string studentId = "another-student-id";

        // Act
        var result = await client.GetAsync($"api/users/students/{studentId}/groups");

        // Arrange
        Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.Forbidden));
    }

    public async Task GetUserById_UserExists_Succeeds()
    {
        // Arrange
        using var client = CreateStudentClient();
        UserViewDto user = new()
        {
            Id = "1",
            Roles = new string[] { Roles.Teacher }
        };
        WebApplicationFactory.UsersServiceMock
            .GetByIdAsync(Arg.Any<string>())
            .Returns(user);

        // Act
        var result = await client.GetAsync($"api/users/{user.Id}");

        // Arrange
        Assert.That(result.IsSuccessStatusCode);
        await WebApplicationFactory.UsersServiceMock
            .Received()
            .GetByIdAsync(user.Id);
    }

    [Test]
    [TestCase(Roles.Administrator)]
    [TestCase(Roles.Teacher)]
    public async Task GetUserById_AdministratorOrTeacherRequestsStudent_Succeeds(string role)
    {
        // Arrange
        using var client = CreateAuthorizedClient(Guid.NewGuid().ToString(), "test-user", role);
        UserViewDto user = new()
        {
            Id = "1",
            Roles = new string[] { Roles.Student }
        };
        WebApplicationFactory.UsersServiceMock
            .GetByIdAsync(Arg.Any<string>())
            .Returns(user);

        // Act
        var result = await client.GetAsync($"api/users/{user.Id}");

        // Arrange
        Assert.That(result.IsSuccessStatusCode);
    }

    [Test]
    public async Task GetUserById_UnauthenticatedUser_Returns401Unauthorized()
    {
        // Arrange
        using var client = CreateUnauthorizedClient();

        // Act
        var result = await client.GetAsync($"api/users/test-id");

        // Arrange
        Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
    }

    [Test]
    public async Task GetUserById_UserDoesNotExist_Returns404NotFound()
    {
        // Arrange
        using var client = CreateAdministratorClient();
        UserViewDto user = new()
        {
            Id = "1"
        };
        WebApplicationFactory.UsersServiceMock
            .GetByIdAsync(Arg.Any<string>())
            .ReturnsNull();

        // Act
        var result = await client.GetAsync($"api/users/{user.Id}");

        // Arrange
        Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
    }

    [Test]
    public async Task GetUserById_StudentRequestsDefaultUser_Returns403Forbidden()
    {
        // Arrange
        using var client = CreateStudentClient();
        UserViewDto user = new()
        {
            Id = "1",
            Roles = new string[] { }
        };
        WebApplicationFactory.UsersServiceMock
            .GetByIdAsync(Arg.Any<string>())
            .Returns(user);

        // Act
        var result = await client.GetAsync($"api/users/{user.Id}");

        // Arrange
        Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.Forbidden));
    }

    [Test]
    public async Task EditUser_ValidCall_Succeeds()
    {
        // Arrange
        EditUserDto dto = new("new-user-name", "email@email.com", "First", "Last");
        using var client = CreateAdministratorClient();
        WebApplicationFactory.UsersServiceMock
            .UpdateUserAsync(Arg.Any<string>(), Arg.Any<EditUserDto>())
            .Returns(true);
        string id = "test-id";

        // Act
        var result = await client.PutAsJsonAsync($"api/users/{id}", dto);

        // Assert
        await WebApplicationFactory.UsersServiceMock
            .Received(1)
            .UpdateUserAsync(id, dto);
        result.EnsureSuccessStatusCode();
    }

    [Test]
    public async Task EditUser_UserDoesNotExist_Returns404NotFound()
    {
        // Arrange
        EditUserDto dto = new("new-user-name", "email@email.com", "First", "Last");
        using var client = CreateAdministratorClient();
        WebApplicationFactory.UsersServiceMock
            .UpdateUserAsync(Arg.Any<string>(), Arg.Any<EditUserDto>())
            .Returns(false);
        string id = "test-id";

        // Act
        var result = await client.PutAsJsonAsync($"api/users/{id}", dto);

        // Assert
        await WebApplicationFactory.UsersServiceMock
            .Received(1)
            .UpdateUserAsync(id, dto);
        Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
    }

    [Test]
    public async Task EditUser_InvalidInput_Returns400BadRequest()
    {
        // Arrange
        EditUserDto invalidDto = new("!", "invalid!", "", "");
        using var client = CreateAdministratorClient();
        WebApplicationFactory.UsersServiceMock
            .UpdateUserAsync(Arg.Any<string>(), Arg.Any<EditUserDto>())
            .Returns(true);
        string id = "test-id";

        // Act
        var result = await client.PutAsJsonAsync($"api/users/{id}", invalidDto);

        // Assert
        Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
    }

    [Test]
    public async Task EditUser_UnauthenticatedUser_Returns401Unauthorized()
    {
        // Arrange
        EditUserDto dto = new("new-user-name", "email@email.com", "First", "Last");
        using var client = CreateUnauthorizedClient();
        WebApplicationFactory.UsersServiceMock
            .UpdateUserAsync(Arg.Any<string>(), Arg.Any<EditUserDto>())
            .Throws(new InvalidOperationException());

        // Act
        var result = await client.PutAsJsonAsync("api/users/test-id", dto);

        // Assert
        Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
    }

    [Test]
    public async Task EditUser_NoAdministratorRole_Returns403Forbidden()
    {
        // Arrange
        EditUserDto dto = new("new-user-name", "email@email.com", "First", "Last");
        using var client = CreateStudentClient();
        WebApplicationFactory.UsersServiceMock
            .UpdateUserAsync(Arg.Any<string>(), Arg.Any<EditUserDto>())
            .Throws(new InvalidOperationException());

        // Act
        var result = await client.PutAsJsonAsync("api/users/test-id", dto);

        // Assert
        Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.Forbidden));
    }

    [Test]
    public async Task GetStudentSemesters_ValidInput_SucceedsAndAppliesFilter()
    {
        // Arrange
        using var client = CreateAdministratorClient();
        string studentId = Guid.NewGuid().ToString();
        WebApplicationFactory.UsersServiceMock
            .GetSemestersOfStudentAsync(Arg.Any<string>(), Arg.Any<PaginationProperties>(),
            Arg.Any<IFilter<Semester>>())
            .Returns(Task.FromResult(new Page<SemesterPreviewDto>()));
        PaginationProperties paginationProperties = new(1, 25);
        SemestersFilterProperties filterProperties = new();

        // Act
        var result = await client.GetAsync($"api/users/students/{studentId}/semesters?page={paginationProperties.Page}&pageSize={paginationProperties.PageSize}");

        // Assert
        result.EnsureSuccessStatusCode();
        var semesters = await result.Content.ReadFromJsonAsync<Page<SemesterPreviewDto>>();
        Assert.That(semesters, Is.Not.Null);
        await WebApplicationFactory.UsersServiceMock
            .Received()
            .GetSemestersOfStudentAsync(studentId, paginationProperties,
            Arg.Is<SemestersFilter>(f => f.Properties == filterProperties));
    }

    [Test]
    public async Task GetStudentSemesters_StudentAccessesAnotherStudentEnrollments_Returns403Forbidden()
    {
        // Arrange
        using var client = CreateStudentClient("student-id");
        string studentId = "another-student-id";

        // Act
        var result = await client.GetAsync($"api/users/students/{studentId}/semesters");

        // Arrange
        Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.Forbidden));
    }

    public async Task EditUserRoles_ValidCall_Succeeds()
    {
        // Arrange
        ChangeRolesDto dto = new(true);
        using var client = CreateAdministratorClient();
        WebApplicationFactory.UsersServiceMock
            .UpdateUserRolesAsync(Arg.Any<string>(), Arg.Any<ChangeRolesDto>())
            .Returns(true);
        string id = "test-id";

        // Act
        var result = await client.PatchAsJsonAsync($"api/users/{id}/roles", dto);

        // Assert
        await WebApplicationFactory.UsersServiceMock
            .Received(1)
            .UpdateUserRolesAsync(id, dto);
        result.EnsureSuccessStatusCode();
    }

    [Test]
    public async Task EditUserRoles_UserDoesNotExist_Returns404NotFound()
    {
        // Arrange
        ChangeRolesDto dto = new(true);
        using var client = CreateAdministratorClient();
        WebApplicationFactory.UsersServiceMock
            .UpdateUserRolesAsync(Arg.Any<string>(), Arg.Any<ChangeRolesDto>())
            .Returns(false);
        string id = "test-id";

        // Act
        var result = await client.PatchAsJsonAsync($"api/users/{id}/roles", dto);

        // Assert
        await WebApplicationFactory.UsersServiceMock
            .Received(1)
            .UpdateUserRolesAsync(id, dto);
        Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
    }

    [Test]
    public async Task EditUserRoles_UnauthenticatedUser_Returns401Unauthorized()
    {
        // Arrange
        ChangeRolesDto dto = new(true);
        using var client = CreateUnauthorizedClient();
        WebApplicationFactory.UsersServiceMock
            .UpdateUserRolesAsync(Arg.Any<string>(), Arg.Any<ChangeRolesDto>())
            .Throws(new InvalidOperationException());

        // Act
        var result = await client.PatchAsJsonAsync("api/users/test-id/roles", dto);

        // Assert
        Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
    }

    [Test]
    public async Task EditUserRoles_NoAdministratorRole_Returns403Forbidden()
    {
        // Arrange
        ChangeRolesDto dto = new(true);
        using var client = CreateStudentClient();
        WebApplicationFactory.UsersServiceMock
            .UpdateUserRolesAsync(Arg.Any<string>(), Arg.Any<ChangeRolesDto>())
            .Throws(new InvalidOperationException());

        // Act
        var result = await client.PatchAsJsonAsync("api/users/test-id/roles", dto);

        // Assert
        Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.Forbidden));
    }

    [Test]
    public async Task DeleteUser_ValidCall_Succeeds()
    {
        // Arrange
        string id = "test-id";
        using var client = CreateAdministratorClient();
        WebApplicationFactory.UsersServiceMock
            .DeleteUserAsync(Arg.Any<string>())
            .Returns(true);

        // Act
        var result = await client.DeleteAsync($"api/users/{id}");

        // Assert
        await WebApplicationFactory.UsersServiceMock
            .Received(1)
            .DeleteUserAsync(id);
        result.EnsureSuccessStatusCode();
    }

    [Test]
    public async Task DeleteUser_UserDoesNotExist_Returns404NotFound()
    {
        // Arrange
        string id = "test-id";
        using var client = CreateAdministratorClient();
        WebApplicationFactory.UsersServiceMock
            .DeleteUserAsync(Arg.Any<string>())
            .Returns(false);

        // Act
        var result = await client.DeleteAsync($"api/users/{id}");

        // Assert
        await WebApplicationFactory.UsersServiceMock
            .Received(1)
            .DeleteUserAsync(id);
        Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
    }

    [Test]
    public async Task DeleteUser_UnauthenticatedUser_Returns401Unauthorized()
    {
        // Arrange
        string id = "test-id";
        using var client = CreateUnauthorizedClient();
        WebApplicationFactory.UsersServiceMock
            .DeleteUserAsync(Arg.Any<string>())
            .Throws(new InvalidOperationException());

        // Act
        var result = await client.DeleteAsync($"api/users/{id}");

        // Assert
        Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
    }

    [Test]
    public async Task DeleteUser_NoAdministratorRole_Returns403Forbidden()
    {
        // Arrange
        string id = "test-id";
        using var client = CreateStudentClient();
        WebApplicationFactory.UsersServiceMock
            .DeleteUserAsync(Arg.Any<string>())
            .Throws(new InvalidOperationException());

        // Act
        var result = await client.DeleteAsync($"api/users/{id}");

        // Assert
        Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.Forbidden));
    }
}
