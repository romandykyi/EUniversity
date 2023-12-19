using EUniversity.Core.Dtos.University;
using EUniversity.Core.Filters;
using EUniversity.Core.Models;
using EUniversity.Core.Models.University;
using EUniversity.Core.Pagination;
using EUniversity.Core.Policy;
using EUniversity.Core.Services.Users;
using EUniversity.Infrastructure.Filters;
using Mapster;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;

namespace EUniversity.IntegrationTests.Services;

public class UsersServiceTests : ServicesTest
{
    private IUsersService _usersService;
    private readonly UsersFilter _usersFilter = new(new("Name", "username123", "email@example.com"));

    // Helper method that adds many users in roles and returns their IDs.
    private async Task<string[]> RegisterManyRolesAsync(int count, params string[] roles)
    {
        string[] result = new string[count];
        for (int i = 0; i < count; i++)
        {
            result[i] = (await RegisterTestUserAsync(roles)).Id;
        }
        return result;
    }

    // Helper method that gets filter for filtering only test users
    private static IFilter<ApplicationUser> GetTestUsersFilter(IEnumerable<string> usersIds)
    {
        HashSet<string> ids = new(usersIds);
        var filter = Substitute.For<IFilter<ApplicationUser>>();
        filter.Apply(Arg.Any<IQueryable<ApplicationUser>>())
            .Returns(x =>
            {
                var query = (IQueryable<ApplicationUser>)x[0]!;
                return query.Where(u => ids.Contains(u.Id));
            });
        return filter;
    }

    [SetUp]
    public void SetUp()
    {
        _usersService = ServiceScope.ServiceProvider.GetService<IUsersService>()!;
    }

    [Test]
    public async Task GetAllUsers_AppliesUsersFilter()
    {
        // Arrange
        var filter = Substitute.For<IFilter<ApplicationUser>>();
        filter.Apply(Arg.Any<IQueryable<ApplicationUser>>())
            .Returns(x => _usersFilter.Apply((IQueryable<ApplicationUser>)x[0]));

        // Act
        await _usersService.GetAllUsersAsync(filter: filter);

        // Assert
        filter
            .Received()
            .Apply(Arg.Any<IQueryable<ApplicationUser>>());
    }

    [Test]
    public async Task GetAllUsers_ReceivesPaginationProperties()
    {
        // Arrange
        PaginationProperties properties = new(3, 20);

        // Act
        var result = await _usersService.GetAllUsersAsync(properties);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result.PageNumber, Is.EqualTo(properties.Page));
            Assert.That(result.PageSize, Is.EqualTo(properties.PageSize));
        });
    }

    [Test]
    public async Task GetAllUsers_ReturnsAllUsers()
    {
        // Arrange
        string[] teachers = await RegisterManyRolesAsync(2, Roles.Teacher);
        string[] students = await RegisterManyRolesAsync(2, Roles.Student);
        var expectedIds = teachers.Concat(students);
        var filter = GetTestUsersFilter(expectedIds);

        // Act
        var result = await _usersService.GetAllUsersAsync(new(1, 20), filter);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result.TotalItemsCount, Is.EqualTo(expectedIds.Count()));
            Assert.That(result.Items.Select(u => u.Id), Is.EquivalentTo(expectedIds));
        });
    }

    [Test]
    public async Task GetUsersInRole_AppliesUsersFilter()
    {
        // Arrange
        var filter = Substitute.For<IFilter<ApplicationUser>>();
        filter.Apply(Arg.Any<IQueryable<ApplicationUser>>())
            .Returns(x => _usersFilter.Apply((IQueryable<ApplicationUser>)x[0]));

        // Act
        await _usersService.GetUsersInRoleAsync(Roles.Student, filter: filter);

        // Assert
        filter
            .Received()
            .Apply(Arg.Any<IQueryable<ApplicationUser>>());
    }

    [Test]
    public async Task GetUsersInRole_ReceivesPaginationProperties()
    {
        // Arrange
        PaginationProperties properties = new(3, PaginationProperties.MinPageSize);

        // Act
        var result = await _usersService.GetUsersInRoleAsync(Roles.Administrator, properties);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result.PageNumber, Is.EqualTo(properties.Page));
            Assert.That(result.PageSize, Is.EqualTo(properties.PageSize));
        });
    }

    [Test]
    public async Task GetUsersInRole_ReturnsUsersInRole()
    {
        // Arrange
        string[] teachers = await RegisterManyRolesAsync(2, Roles.Teacher);
        string[] students = await RegisterManyRolesAsync(2, Roles.Student);
        var expectedIds = teachers;
        var filter = GetTestUsersFilter(expectedIds);

        // Act
        var result = await _usersService.GetUsersInRoleAsync(Roles.Teacher, new(1, 20), filter);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result.TotalItemsCount, Is.EqualTo(expectedIds.Length));
            Assert.That(result.Items.Select(u => u.Id), Is.EquivalentTo(expectedIds));
        });
    }

    [Test]
    public async Task GetGroupsOfStudent_ReturnsGroupsOfStudent()
    {
        // Arrange
        var student = await RegisterTestUserAsync(Roles.Student);
        Course course = new()
        {
            Name = "Some course"
        };
        DbContext.Add(course);
        await DbContext.SaveChangesAsync();
        Group group1 = new()
        {
            Name = "G1",
            CourseId = course.Id
        };
        Group group2 = new()
        {
            Name = "G2",
            CourseId = course.Id
        };
        DbContext.Add(group1);
        DbContext.Add(group2);
        await DbContext.SaveChangesAsync();
        // Add student to a group
        StudentGroup studentGroup = new()
        {
            StudentId = student.Id,
            GroupId = group1.Id
        };
        DbContext.Add(studentGroup);
        await DbContext.SaveChangesAsync();
        GroupPreviewDto[] expectedResult =
        {
            group1.Adapt<GroupPreviewDto>()
        };

        // Act
        var result = await _usersService
            .GetGroupsOfStudentAsync(student.Id, new PaginationProperties());

        // Assert
        Assert.That(result.Items, Is.EquivalentTo(expectedResult));
    }

    [Test]
    public async Task GetGroupsOfStudent_AppliesFilter()
    {
        // Arrange
        var student = await RegisterTestUserAsync();
        IFilter<Group> filterMock = Substitute.For<IFilter<Group>>();
        filterMock
            .Apply(Arg.Any<IQueryable<Group>>())
            .Returns(x => x.ArgAt<IQueryable<Group>>(0));

        // Act
        await _usersService.GetGroupsOfStudentAsync(student.Id, new PaginationProperties(), filterMock);

        // Assert
        filterMock
            .Received()
            .Apply(Arg.Any<IQueryable<Group>>());
    }

    [Test]
    public async Task GetSemestersOfStudent_ReturnsSemestersOfStudent()
    {
        // Arrange
        var student = await RegisterTestUserAsync(Roles.Student);
        Semester semester1 = new()
        {
            Name = "S1"
        };
        Semester semester2 = new()
        {
            Name = "S2"
        };
        DbContext.Add(semester1);
        DbContext.Add(semester2);
        await DbContext.SaveChangesAsync();
        // Add student to a semester
        StudentSemester studentSemester = new()
        {
            StudentId = student.Id,
            SemesterId = semester1.Id
        };
        DbContext.Add(studentSemester);
        await DbContext.SaveChangesAsync();
        SemesterPreviewDto[] expectedResult =
        {
            semester1.Adapt<SemesterPreviewDto>()
        };

        // Act
        var result = await _usersService
            .GetSemestersOfStudentAsync(student.Id, new PaginationProperties());

        // Assert
        Assert.That(result.Items, Is.EquivalentTo(expectedResult));
    }

    [Test]
    public async Task GetSemestersOfStudent_AppliesFilter()
    {
        // Arrange
        var student = await RegisterTestUserAsync();
        IFilter<Semester> filterMock = Substitute.For<IFilter<Semester>>();
        filterMock
            .Apply(Arg.Any<IQueryable<Semester>>())
            .Returns(x => x.ArgAt<IQueryable<Semester>>(0));

        // Act
        await _usersService.GetSemestersOfStudentAsync(student.Id, new PaginationProperties(), filterMock);

        // Assert
        filterMock
            .Received()
            .Apply(Arg.Any<IQueryable<Semester>>());
    }
}
