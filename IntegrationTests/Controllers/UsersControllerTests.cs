﻿using Duende.IdentityServer.Test;
using EUniversity.Core.Dtos.Auth;
using EUniversity.Core.Dtos.Users;
using EUniversity.Core.Filters;
using EUniversity.Core.Models;
using EUniversity.Core.Pagination;
using EUniversity.Core.Policy;
using EUniversity.Core.Services.Auth;
using IdentityModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using NSubstitute;
using NSubstitute.ReceivedExtensions;
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

    public UserViewDto[] TestUsers =
    {
        new("1", "mail1@example.com", "user1", "First1", "Last1", null),
        new("2", "mail2@example.com", "user2", "First2", "Last2", "Middle2")
    };

    public UsersFilterProperties TestFilterProperties = new("Joe Doe", "joedoe777", "mail@example.com");
    public const string TestFilterQuery = "fullName=Joe%20Doe&userName=joedoe777&email=mail@example.com";

    public static readonly string[] GetMethods =
    {
        "/api/users",
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

    private Page<UserViewDto> GetTestPage(PaginationProperties paginationProperties)
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
            .GetAllUsersAsync(Arg.Any<PaginationProperties>(), Arg.Any<IFilter<ApplicationUser>>())
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
        var users = await result.Content.ReadFromJsonAsync<Page<UserViewDto>>();
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
        var usersPage = await result.Content.ReadFromJsonAsync<Page<UserViewDto>>();
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
        var usersPage = await result.Content.ReadFromJsonAsync<Page<UserViewDto>>();
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
        var usersPage = await result.Content.ReadFromJsonAsync<Page<UserViewDto>>();
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
    [TestCaseSource(nameof(GetMethods))]
    public async Task GetMethods_NoAdministratorRole_Return403Forbidden(string method)
    {
        // Arrange
        using var client = CreateStudentClient();

        // Act
        var result = await client.GetAsync(method);

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
}
