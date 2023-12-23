﻿using EUniversity.Core.Filters;
using EUniversity.Core.Models;
using EUniversity.Core.Pagination;
using EUniversity.Core.Policy;
using EUniversity.Core.Services.Users;
using EUniversity.Infrastructure.Filters;
using Microsoft.EntityFrameworkCore;
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
    public virtual async Task DeleteUser_UserExists_Succeeds()
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
    public virtual async Task DeleteUser_UserDoesNotExist_ReturnsFalse()
    {
        // Arrange
        string fakeId = "null";

        // Act
        bool result = await _usersService.DeleteUserAsync(fakeId);

        // Assert
        Assert.That(result, Is.False);
    }

    [Test]
    public virtual async Task DeleteUser_UserIsDeleted_ReturnsFalse()
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
}
