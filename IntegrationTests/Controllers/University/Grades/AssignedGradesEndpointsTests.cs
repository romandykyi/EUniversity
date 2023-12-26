using EUniversity.Core.Dtos.University;
using EUniversity.Core.Dtos.University.Grades;
using EUniversity.Core.Filters;
using EUniversity.Core.Models;
using EUniversity.Core.Models.University;
using EUniversity.Core.Models.University.Grades;
using EUniversity.Core.Pagination;
using EUniversity.Core.Services.University;
using EUniversity.Infrastructure.Filters;
using IdentityModel;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using System.Net;

namespace EUniversity.IntegrationTests.Controllers.University.Grades;

public class AssignedGradesEndpointsTests : ControllersTest
{
    private const string TestStudentId = "student-id";
    private const string TestTeacherId = "teacher-id";
    private const int TestGroupId = 500;

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
        WebApplicationFactory.GroupsServiceMock
            .GetOwnerIdAsync(Arg.Is<int>(x => x != TestGroupId))
            .Throws(new InvalidOperationException("GetOwnerIDAsync is called with invalid argument"));
        WebApplicationFactory.GroupsServiceMock
            .GetOwnerIdAsync(TestGroupId)
            .Returns(new GetOwnerIdResponse(true, TestTeacherId));

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
}
