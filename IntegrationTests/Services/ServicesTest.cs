using EUniversity.Core.Models;
using EUniversity.Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace EUniversity.IntegrationTests.Services;

/// <summary>
/// Base class for isolated integration tests of services against local/in-memory db.
/// </summary>
/// <remarks>
/// Each test in derived classes is isolated, and no changes to the database will be saved.
/// </remarks>
public abstract class ServicesTest : IntegrationTest<ProgramWebApplicationFactory>
{
    protected ApplicationDbContext DbContext { get; private set; }

    [SetUp]
    public async Task SetUpTransaction()
    {
        DbContext = ServiceScope.ServiceProvider.GetService<ApplicationDbContext>()!;
        // Begin transaction to make tests isolated
        await DbContext.Database.BeginTransactionAsync();
    }

    [TearDown]
    public void TearDownTransaction()
    {
        // Undo all changes
        DbContext.ChangeTracker.Clear();
        DbContext.Database.RollbackTransaction();
    }

    /// <summary>
    /// Registers a test user.
    /// </summary>
    /// <param name="roles">Roles that will be assigned to the user.</param>
    /// <returns>
    /// <see cref="ApplicationUser"/> that exists in the test database.
    /// </returns>
    protected async Task<ApplicationUser> RegisterTestUser(params string[] roles)
    {
        ApplicationUser user = new()
        {
            UserName = "test-user",
            FirstName = "Test1",
            LastName = "Test2",
            Email = "test@example.com"
        };
        var userManager = ServiceScope.ServiceProvider.GetService<UserManager<ApplicationUser>>()!;
        await userManager.CreateAsync(user, "StrongPa$$w0rd");
        await userManager.AddToRolesAsync(user, roles);

        return user;
    }
}
