using EUniversity.Core.Dtos.Auth;
using EUniversity.Core.Models;
using EUniversity.Core.Policy;
using EUniversity.Core.Services.Auth;
using EUniversity.Infrastructure.Services.Test;
using Microsoft.AspNetCore.Identity;
using System.Reflection;

namespace EUniversity.Extensions;

public static class WebApplicationExtensions
{
    public static WebApplication CreateRoles(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();

        var roleManager =
            scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

        // Select all constants from Roles class
        var roles = typeof(Roles)
            .GetFields(BindingFlags.Public | BindingFlags.Static)
            .Where(f => f.IsLiteral && !f.IsInitOnly)
            .Select(f => f.GetRawConstantValue()!.ToString()!);

        foreach (var role in roles)
        {
            if (!roleManager.RoleExistsAsync(role).Result)
            {
                roleManager.CreateAsync(new(role)).Wait();
            }
        }
        return app;
    }

    public static WebApplication CreateAdministrator(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();

        var userManager =
            scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        var authService =
            scope.ServiceProvider.GetRequiredService<IAuthService>();

        const string adminUserName = "admin";
        const string adminPassword = "Chang3M3InProduct10nPlz!";
        RegisterDto registerDto = new("admin@e-university.com", "Admino", "Guro");

        if (userManager.FindByNameAsync(adminUserName).Result == null)
        {
            authService.RegisterAsync(registerDto, adminUserName, adminPassword, Roles.Administrator).Wait();
        }
        return app;
    }

    public static WebApplication CreateTestUsers(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();

        var testDataService =
            scope.ServiceProvider.GetRequiredService<TestDataService>();

        testDataService.CreateTestUsersAsync().Wait();

        return app;
    }

    public static WebApplication CreateFakeData(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();

        var testDataService =
            scope.ServiceProvider.GetRequiredService<TestDataService>();

        testDataService.CreateFakeUsersAsync().Wait();
        testDataService.CreateFakeClassroomsAsync().Wait();
        testDataService.CreateFakeSemestersAsync().Wait();
        testDataService.CreateFakeGradesAsync().Wait();
        testDataService.CreateFakeCoursesAsync().Wait();
        testDataService.CreateFakeGroupsAsync().Wait();
        testDataService.CreateTestClassesTypesAsync().Wait();
        testDataService.CreateFakeClassesAsync().Wait();
        testDataService.CreateTestActivityTypesAsync().Wait();

        return app;
    }
}
