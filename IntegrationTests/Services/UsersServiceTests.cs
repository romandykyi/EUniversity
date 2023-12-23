using EUniversity.Core.Dtos.Users;
﻿using EUniversity.Core.Dtos.University;
using EUniversity.Core.Filters;
using EUniversity.Core.Models;
using EUniversity.Core.Models.University;
using EUniversity.Core.Pagination;
using EUniversity.Core.Policy;
using EUniversity.Core.Services.Users;
using EUniversity.Infrastructure.Filters;
using Microsoft.EntityFrameworkCore;
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
    public async Task GetAllUsers_OnlyDeleted_ReturnsOnlyDeletedUsers()
    {
        // Arrange
        string[] teachers = await RegisterManyRolesAsync(2, Roles.Teacher);
        string[] students = await RegisterManyRolesAsync(2, Roles.Student);
        HashSet<string> allUsersIds = new(teachers.Concat(students));
        foreach (var user in DbContext.Users.Where(u => allUsersIds.Contains(u.Id)))
        {
            user.IsDeleted = true;
        }
        await DbContext.SaveChangesAsync();
        var filter = GetTestUsersFilter(allUsersIds);

        // Act
        var result = await _usersService.GetAllUsersAsync(new(1, 20), filter, true);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result.TotalItemsCount, Is.EqualTo(allUsersIds.Count));
            Assert.That(result.Items.Select(u => u.Id), Is.EquivalentTo(allUsersIds));
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
    public async Task DeleteUser_UserExists_Succeeds()
    {
        // Arrange
        ApplicationUser user = new()
        {
            FirstName = "Test",
            LastName = "User",
            Email = "test-user@example.com",
            IsDeleted = false
        };
        DbContext.Add(user);
        await DbContext.SaveChangesAsync();

        // Act
        bool result = await _usersService.DeleteUserAsync(user.Id);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result);
            Assert.That(user.IsDeleted);
        });
    }

    [Test]
    public async Task DeleteUser_UserDoesNotExist_ReturnsFalse()
    {
        // Arrange
        string fakeId = "null";

        // Act
        bool result = await _usersService.DeleteUserAsync(fakeId);

        // Assert
        Assert.That(result, Is.False);
    }

    [Test]
    public async Task DeleteUser_UserIsDeleted_ReturnsFalse()
    {
        // Arrange
        ApplicationUser user = new()
        {
            FirstName = "Test",
            LastName = "User",
            Email = "test-user@example.com",
            IsDeleted = true
        };
        DbContext.Add(user);
        await DbContext.SaveChangesAsync();

        // Act
        bool result = await _usersService.DeleteUserAsync(user.Id);

        // Assert
        Assert.That(result, Is.False);
    }

    [Test]
    public async Task GetUserById_UserExists_ReturnsUserWithRoles()
    {
        // Arrange
        string[] roles = { Roles.Administrator, Roles.Teacher };
        var user = await RegisterTestUserAsync(roles);

        // Act
        var result = await _usersService.GetByIdAsync(user.Id);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(result.Id, Is.EqualTo(user.Id));
            Assert.That(result.FirstName, Is.EqualTo(user.FirstName));
            Assert.That(result.Email, Is.EqualTo(user.Email));
            Assert.That(result.Roles, Is.EquivalentTo(roles));
        });
    }

    [Test]
    public async Task GetUserById_UserDoesNotExist_ReturnsNull()
    {
        // Arrange
        string fakeId = "null";

        // Act
        var result = await _usersService.GetByIdAsync(fakeId);

        // Assert
        Assert.That(result, Is.Null);
    }

    [Test]
    public virtual async Task UpdateUser_UserExists_Succeeds()
    {
        // Arrange
        var user = await RegisterTestUserAsync();
        EditUserDto editDto = new("new-user-name", "new@email.com", "NewName", "NewLastName", "NewMiddleName");

        // Act
        bool result = await _usersService.UpdateUserAsync(user.Id, editDto);

        // Assert
        Assert.That(result);
        // Assert that user was updated
        Assert.Multiple(() =>
        {
            Assert.That(user.UserName, Is.EqualTo(editDto.UserName));
            Assert.That(user.Email, Is.EqualTo(editDto.Email));
            Assert.That(user.FirstName, Is.EqualTo(editDto.FirstName));
            Assert.That(user.LastName, Is.EqualTo(editDto.LastName));
            Assert.That(user.MiddleName, Is.EqualTo(editDto.MiddleName));
        });
    }

    [Test]
    public virtual async Task UpdateUser_UserDoesNotExist_ReturnsFalse()
    {
        // Arrange
        EditUserDto editDto = new("new-user-name", "new@email.com", "NewName", "NewLastName", "NewMiddleName");

        // Act
        bool result = await _usersService.UpdateUserAsync("null", editDto);

        // Assert
        Assert.That(result, Is.False);
    }

    [Test]
    public virtual async Task UpdateUser_UserIsDeleted_ReturnsFalse()
    {
        // Arrange
        var user = await RegisterTestUserAsync();
        user.IsDeleted = true;
        await DbContext.SaveChangesAsync();
        EditUserDto editDto = new("new-user-name", "new@email.com", "NewName", "NewLastName", "NewMiddleName");

        // Act
        bool result = await _usersService.UpdateUserAsync(user.Id, editDto);

        // Assert
        Assert.That(result, Is.False);
    }

    [Test]
    public virtual async Task UpdateUserRoles_UserExists_Succeeds()
    {
        // Arrange
        var user = await RegisterTestUserAsync(Roles.Teacher);
        string[] expectedRoles = { Roles.Administrator };
        ChangeRolesDto dto = new(IsTeacher: false, IsAdministrator: true);

        // Act
        bool result = await _usersService.UpdateUserRolesAsync(user.Id, dto);

        // Assert
        Assert.That(result);
        List<string> actualRoles = await DbContext.UserRoles
            .AsNoTracking()
            .Where(ur => ur.UserId == user.Id)
            .Join(DbContext.Roles, ur => ur.RoleId, r => r.Id, (ur, r) => r)
            .Select(r => r.Name!)
            .ToListAsync();
        Assert.That(actualRoles, Is.EquivalentTo(expectedRoles));
    }

    [Test]
    public virtual async Task UpdateUserRoles_UserDoesNotExist_ReturnsFalse()
    {
        // Arrange
        ChangeRolesDto dto = new();

        // Act
        bool result = await _usersService.UpdateUserRolesAsync("null", dto);

        // Assert
        Assert.That(result, Is.False);
    }

    [Test]
    public virtual async Task UpdateUserRoles_UserIsDeleted_ReturnsFalse()
    {
        // Arrange
        var user = await RegisterTestUserAsync();
        user.IsDeleted = true;
        await DbContext.SaveChangesAsync();
        ChangeRolesDto dto = new();

        // Act
        bool result = await _usersService.UpdateUserRolesAsync(user.Id, dto);

        // Assert
        Assert.That(result, Is.False);
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
