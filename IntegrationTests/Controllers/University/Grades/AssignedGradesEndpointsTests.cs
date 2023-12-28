using EUniversity.Core.Dtos.University.Grades;
using EUniversity.Core.Filters;
using EUniversity.Core.Models.University.Grades;
using EUniversity.Core.Pagination;
using EUniversity.Core.Policy;
using EUniversity.Core.Services.University;
using EUniversity.Core.Services.University.Grades;
using EUniversity.Infrastructure.Filters;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using System.Net;

namespace EUniversity.IntegrationTests.Controllers.University.Grades;

public class AssignedGradesEndpointsTests : ControllersTest
{
    private const string TestStudentId = "student-id";
    private const string TestTeacherId = "teacher-id";
    private const int TestGroupId = 500;
    private const int TestAssignedGradeId = 600;

    private static readonly string[] GetEndpoints =
    {
        $"api/users/students/{TestStudentId}/grades",
        $"api/groups/{TestGroupId}/grades",
        $"api/groups/{TestGroupId}/students/{TestStudentId}/grades",
        $"api/assignedGrades"
    };

    protected virtual Page<AssignedGradeViewDto> GetTestPreviewDtos(PaginationProperties properties)
    {
        IEnumerable<AssignedGradeViewDto> testEnumerable =
            Enumerable.Repeat(new AssignedGradeViewDto(
                1, null, new(1, "1"), new(2, "2", new(3, "3", null)),
                new(), null, null, DateTimeOffset.Now, DateTimeOffset.Now, null), 10);
        return new(testEnumerable, properties, 100);
    }

    [SetUp]
    public void SetUp()
    {
        SetUpValidationMocks();
        WebApplicationFactory.GroupsServiceMock
            .GetOwnerIdAsync(Arg.Is<int>(x => x != TestGroupId))
            .Returns(new GetOwnerIdResponse(false, null));
        WebApplicationFactory.GroupsServiceMock
            .GetOwnerIdAsync(TestGroupId)
            .Returns(new GetOwnerIdResponse(true, TestTeacherId));

        WebApplicationFactory.AssignedGradesServiceMock
            .GetAssignerIdAsync(Arg.Is<int>(x => x != TestAssignedGradeId))
            .Returns(new GetAssignerIdResponse(false, null));
        WebApplicationFactory.AssignedGradesServiceMock
            .GetAssignerIdAsync(Arg.Any<int>())
            .Returns(new GetAssignerIdResponse(true, TestTeacherId));

        WebApplicationFactory.AssignedGradesServiceMock
            .GetPageAsync<AssignedGradeViewDto>(Arg.Any<PaginationProperties>(), Arg.Any<IFilter<AssignedGrade>>(),
            Arg.Any<bool>(), Arg.Any<bool>())
            .Returns(x => Task.FromResult(GetTestPreviewDtos((PaginationProperties)x[0])));
    }

    [Test]
    [TestCaseSource(nameof(GetEndpoints))]
    public async Task GetMethods_SucceedAndReturnValidDtos(string method)
    {
        // Arrange
        using var client = CreateAdministratorClient();
        const int page = 2, pageSize = 25;

        // Act
        var result = await client.GetAsync($"{method}?page={page}&pageSize={pageSize}");

        // Assert
        result.EnsureSuccessStatusCode();
        var resultPage = await result.Content.ReadFromJsonAsync<Page<AssignedGradeViewDto>>();
        Assert.That(resultPage, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(resultPage.PageNumber, Is.EqualTo(page));
            Assert.That(resultPage.PageSize, Is.EqualTo(pageSize));
            Assert.That(resultPage.Items.Count(), Is.LessThanOrEqualTo(pageSize));
        });
    }

    [Test]
    [TestCaseSource(nameof(GetEndpoints))]
    public async Task GetMethods_UnauthorizedCall_Returns401Unauthorized(string method)
    {
        // Arrange
        using var client = CreateUnauthorizedClient();

        // Act
        var result = await client.GetAsync(method);

        // Assert
        Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
    }

    [Test]
    [TestCaseSource(nameof(GetEndpoints))]
    public async Task GetMethods_NoAdministratorRole_Returns403Forbidden(string method)
    {
        // Arrange
        using var client = CreateStudentClient();

        // Act
        var result = await client.GetAsync(method);

        // Assert
        Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.Forbidden));
    }

    [Test]
    public async Task GetGradesOfStudent_StudentAccessesOwnGrades_Succeeds()
    {
        // Arrange
        using var client = CreateStudentClient(TestStudentId);

        // Act
        var result = await client.GetAsync($"api/users/students/{TestStudentId}/grades");

        // Assert
        result.EnsureSuccessStatusCode();
    }

    [Test]
    public async Task GetGradesOfStudent_AppliesFilter()
    {
        // Arrange
        using var client = CreateAdministratorClient();

        // Act
        var result = await client.GetAsync($"api/users/students/{TestStudentId}/grades");

        // Assert
        result.EnsureSuccessStatusCode();
        await WebApplicationFactory.AssignedGradesServiceMock
            .Received(1)
            .GetPageAsync<AssignedGradeViewDto>(
                Arg.Any<PaginationProperties>(),
                Arg.Is<AssignedGradesFilter>(x => x.StudentId == TestStudentId),
                true, false);
    }

    [Test]
    public async Task GetGradesInGroup_OtherTeacherAccessesGrades_Returns403Forbidden()
    {
        // Arrange
        using var client = CreateTeacherClient();

        // Act
        var result = await client.GetAsync($"api/groups/{TestGroupId}/grades");

        // Assert
        Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.Forbidden));
    }

    [Test]
    public async Task GetGradesInGroup_OwnerAccessesGrades_Succeeds()
    {
        // Arrange
        using var client = CreateTeacherClient(TestTeacherId);

        // Act
        var result = await client.GetAsync($"api/groups/{TestGroupId}/grades");

        // Assert
        result.EnsureSuccessStatusCode();
    }

    [Test]
    public async Task GetGradesInGroup_AppliesFilter()
    {
        // Arrange
        using var client = CreateAdministratorClient();

        // Act
        var result = await client.GetAsync($"api/groups/{TestGroupId}/grades");

        // Assert
        result.EnsureSuccessStatusCode();
        await WebApplicationFactory.AssignedGradesServiceMock
            .Received()
            .GetPageAsync<AssignedGradeViewDto>(
                Arg.Any<PaginationProperties>(),
                Arg.Is<AssignedGradesFilter>(x => x.GroupId == TestGroupId),
                false, true);
    }

    [Test]
    public async Task GetGradesOfStudentInGroup_StudentAccessesOwnGrades_Succeeds()
    {
        // Arrange
        using var client = CreateStudentClient(TestStudentId);

        // Act
        var result = await client.GetAsync($"api/groups/{TestGroupId}/students/{TestStudentId}/grades");

        // Assert
        result.EnsureSuccessStatusCode();
    }

    [Test]
    public async Task GetGradesOfStudentInGroup_OtherTeacherAccessesGrades_Returns403Forbidden()
    {
        // Arrange
        using var client = CreateTeacherClient();

        // Act
        var result = await client.GetAsync($"api/groups/{TestGroupId}/students/{TestStudentId}/grades");

        // Assert
        Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.Forbidden));
    }

    [Test]
    public async Task GetGradesOfStudentInGroup_OwnerAccessesGrades_Succeeds()
    {
        // Arrange
        using var client = CreateTeacherClient(TestTeacherId);

        // Act
        var result = await client.GetAsync($"api/groups/{TestGroupId}/students/{TestStudentId}/grades");

        // Assert
        result.EnsureSuccessStatusCode();
    }

    [Test]
    public async Task GetGradesOfStudentInGroup_AppliesFilter()
    {
        // Arrange
        using var client = CreateAdministratorClient();

        // Act
        var result = await client.GetAsync($"api/groups/{TestGroupId}/students/{TestStudentId}/grades");

        // Assert
        result.EnsureSuccessStatusCode();
        await WebApplicationFactory.AssignedGradesServiceMock
            .Received(1)
            .GetPageAsync<AssignedGradeViewDto>(
                Arg.Any<PaginationProperties>(),
                Arg.Is<AssignedGradesFilter>(x => x.StudentId == TestStudentId && x.GroupId == TestGroupId),
                false, false);
    }

    [Test]
    public async Task Post_ValidCall_Succeeds()
    {
        // Arrange
        AssignedGradeCreateDto validCreateDto = new(1, TestGroupId, TestStudentId, null, null);
        string userId = "userId";
        using var client = CreateAdministratorClient(userId);
        WebApplicationFactory.AssignedGradesServiceMock
            .AssignAsync(Arg.Any<AssignedGradeCreateDto>(), Arg.Any<string>())
            .Returns(new AssignedGrade());

        // Act
        var result = await client.PostAsJsonAsync("api/assignedGrades", validCreateDto);

        // Assert
        result.EnsureSuccessStatusCode();
        await WebApplicationFactory.AssignedGradesServiceMock
            .Received()
            .AssignAsync(validCreateDto, userId);
    }

    [Test]
    public async Task Post_UnauthenticatedUser_Returns401Unauthorized()
    {
        // Arrange
        AssignedGradeCreateDto validCreateDto = new(1, TestGroupId, TestStudentId, null, null);
        using var client = CreateUnauthorizedClient();
        WebApplicationFactory.AssignedGradesServiceMock
            .AssignAsync(Arg.Any<AssignedGradeCreateDto>(), Arg.Any<string>())
            .Throws<InvalidOperationException>();

        // Act
        var result = await client.PostAsJsonAsync("api/assignedGrades", validCreateDto);

        // Assert
        Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
    }

    [Test]
    [TestCase(Roles.Student)]
    [TestCase(Roles.Teacher)]
    public async Task Post_NotOwner_Returns403Forbidden(string role)
    {
        // Arrange
        AssignedGradeCreateDto validCreateDto = new(1, TestGroupId, TestStudentId, null, null);
        using var client = CreateAuthorizedClient("id", "userName", role);
        WebApplicationFactory.AssignedGradesServiceMock
            .AssignAsync(Arg.Any<AssignedGradeCreateDto>(), Arg.Any<string>())
            .Throws<InvalidOperationException>();

        // Act
        var result = await client.PostAsJsonAsync("api/assignedGrades", validCreateDto);

        // Assert
        Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.Forbidden));
    }

    [Test]
    public async Task Post_Owner_Succeeds()
    {
        // Arrange
        AssignedGradeCreateDto validCreateDto = new(1, TestGroupId, TestStudentId, null, null);
        using var client = CreateTeacherClient(TestTeacherId);
        WebApplicationFactory.AssignedGradesServiceMock
            .AssignAsync(Arg.Any<AssignedGradeCreateDto>(), Arg.Any<string>())
            .Returns(new AssignedGrade());

        // Act
        var result = await client.PostAsJsonAsync("api/assignedGrades", validCreateDto);

        // Assert
        result.EnsureSuccessStatusCode();
    }

    [Test]
    public async Task Put_ValidCall_Succeeds()
    {
        // Arrange
        AssignedGradeUpdateDto dto = new(1, null, null);
        const string userId = "userId";
        using var client = CreateAdministratorClient(userId);
        WebApplicationFactory.AssignedGradesServiceMock
            .ReassignAsync(Arg.Any<int>(), Arg.Any<AssignedGradeUpdateDto>(), Arg.Any<string>())
            .Returns(true);

        // Act
        var result = await client.PutAsJsonAsync($"api/assignedGrades/{TestAssignedGradeId}", dto);

        // Assert
        await WebApplicationFactory.AssignedGradesServiceMock
            .Received()
            .ReassignAsync(TestAssignedGradeId, dto, userId);
        result.EnsureSuccessStatusCode();
    }

    [Test]
    public async Task Put_ElementDoesNotExist_Returns404NotFound()
    {
        // Arrange
        AssignedGradeUpdateDto dto = new(1, null, null);
        using var client = CreateAdministratorClient();
        WebApplicationFactory.AssignedGradesServiceMock
            .ReassignAsync(Arg.Any<int>(), Arg.Any<AssignedGradeUpdateDto>(), Arg.Any<string>())
            .Returns(false);

        // Act
        var result = await client.PutAsJsonAsync($"api/assignedGrades/{TestAssignedGradeId}", dto);

        // Assert
        Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
    }

    [Test]
    public async Task Put_UnauthenticatedUser_Returns401Unauthorized()
    {
        // Arrange
        AssignedGradeUpdateDto dto = new(1, null, null);
        using var client = CreateUnauthorizedClient();
        WebApplicationFactory.AssignedGradesServiceMock
            .ReassignAsync(Arg.Any<int>(), Arg.Any<AssignedGradeUpdateDto>(), Arg.Any<string>())
            .Throws<InvalidOperationException>();

        // Act
        var result = await client.PutAsJsonAsync($"api/assignedGrades/{TestAssignedGradeId}", dto);

        // Assert
        Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
    }

    [Test]
    [TestCase(Roles.Teacher)]
    [TestCase(Roles.Student)]
    public async Task Put_NotAssigner_Returns403Forbidden(string role)
    {
        // Arrange
        AssignedGradeUpdateDto dto = new(1, null, null);
        using var client = CreateAuthorizedClient("userId", "userName", role);
        WebApplicationFactory.AssignedGradesServiceMock
            .ReassignAsync(Arg.Any<int>(), Arg.Any<AssignedGradeUpdateDto>(), Arg.Any<string>())
            .Throws<InvalidOperationException>();

        // Act
        var result = await client.PutAsJsonAsync($"api/assignedGrades/{TestAssignedGradeId}", dto);

        // Assert
        Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.Forbidden));
    }

    [Test]
    public async Task Put_AssignerTeacher_Succeeds()
    {
        // Arrange
        AssignedGradeUpdateDto dto = new(1, null, null);
        using var client = CreateTeacherClient(TestTeacherId);
        WebApplicationFactory.AssignedGradesServiceMock
            .ReassignAsync(Arg.Any<int>(), Arg.Any<AssignedGradeUpdateDto>(), Arg.Any<string>())
            .Returns(true);

        // Act
        var result = await client.PutAsJsonAsync($"api/assignedGrades/{TestAssignedGradeId}", dto);

        // Assert
        result.EnsureSuccessStatusCode();
    }

    [Test]
    public async Task Delete_ValidCall_Succeeds()
    {
        // Arrange
        using var client = CreateAdministratorClient();
        WebApplicationFactory.AssignedGradesServiceMock
            .DeleteAsync(Arg.Any<int>())
            .Returns(true);

        // Act
        var result = await client.DeleteAsync($"api/assignedGrades/{TestAssignedGradeId}");

        // Assert
        await WebApplicationFactory.AssignedGradesServiceMock
            .Received()
            .DeleteAsync(TestAssignedGradeId);
        result.EnsureSuccessStatusCode();
    }

    [Test]
    public async Task Delete_ElementDoesNotExist_Return404NotFound()
    {
        // Arrange
        using var client = CreateAdministratorClient();
        WebApplicationFactory.AssignedGradesServiceMock
            .DeleteAsync(Arg.Any<int>())
            .Returns(false);

        // Act
        var result = await client.DeleteAsync($"api/assignedGrades/{TestAssignedGradeId}");

        // Assert
        await WebApplicationFactory.AssignedGradesServiceMock
            .Received()
            .DeleteAsync(TestAssignedGradeId);
        Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
    }

    [Test]
    public async Task Delete_UnauthenticatedUser_Returns401Unauthorized()
    {
        // Arrange
        using var client = CreateUnauthorizedClient();
        WebApplicationFactory.AssignedGradesServiceMock
            .DeleteAsync(Arg.Any<int>())
            .Throws<InvalidOperationException>();

        // Act
        var result = await client.DeleteAsync($"api/assignedGrades/{TestAssignedGradeId}");

        // Assert
        Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
    }

    [Test]
    [TestCase(Roles.Teacher)]
    [TestCase(Roles.Student)]
    public async Task Delete_NotAssigner_Returns403Forbidden(string role)
    {
        // Arrange
        using var client = CreateAuthorizedClient("userId", "userName", role);
        WebApplicationFactory.AssignedGradesServiceMock
            .DeleteAsync(Arg.Any<int>())
            .Throws<InvalidOperationException>();

        // Act
        var result = await client.DeleteAsync($"api/assignedGrades/{TestAssignedGradeId}");

        // Assert
        Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.Forbidden));
    }

    [Test]
    public async Task Delete_AssignerTeacher_Succeeds()
    {
        // Arrange
        using var client = CreateTeacherClient(TestTeacherId);
        WebApplicationFactory.AssignedGradesServiceMock
            .DeleteAsync(Arg.Any<int>())
            .Returns(true);

        // Act
        var result = await client.DeleteAsync($"api/assignedGrades/{TestAssignedGradeId}");

        // Assert
        result.EnsureSuccessStatusCode();
    }
}
