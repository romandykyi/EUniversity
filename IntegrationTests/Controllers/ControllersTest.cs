using EUniversity.Core.Models;
using EUniversity.Core.Policy;
using EUniversity.IntegrationTests.Mocks;
using NSubstitute;

namespace EUniversity.IntegrationTests.Controllers;

/// <summary>
/// Base class for integration tests of controllers with mocked services.
/// </summary>
public class ControllersTest : IntegrationTest<MockedProgramWebApplicationFactory>
{
    public HttpClient CreateUnauthorizedClient() =>
        WebApplicationFactory.CreateCustomClient();

    public HttpClient CreateAuthorizedClient(string? id, string userName, params string[] roles)
    {
        WebApplicationFactory.ClaimsProvider.Init(id, userName, roles);
        return WebApplicationFactory.CreateCustomClient();
    }

    public HttpClient CreateAdministratorClient(string? id = null) =>
        CreateAuthorizedClient(id, "admin", Roles.Administrator);
    public HttpClient CreateStudentClient(string? id = null) =>
        CreateAuthorizedClient(id, "student", Roles.Student);
    public HttpClient CreateTeacherClient(string? id = null) =>
        CreateAuthorizedClient(id, "teacher", Roles.Teacher);

    /// <summary>
    /// Sets up validation mocks in such a way that all
    /// foreign key validations pass.
    /// </summary>
    protected void SetUpValidationMocks()
    {
        WebApplicationFactory.ExistenceCheckerMock
            .ExistsAsync<AnyEntity, AnyEntityId>(Arg.Any<AnyEntityId>())
            .ReturnsForAnyArgs(true);

        WebApplicationFactory.UserManagerMock
            .FindByIdAsync(Arg.Any<string>())
            .Returns(new ApplicationUser()
            {
                Id = "test",
                Email = "a@example.com",
                FirstName = "Test1",
                LastName = "Test2"
            });

        WebApplicationFactory.UserManagerMock
            .IsInRoleAsync(Arg.Any<ApplicationUser>(), Arg.Any<string>())
            .Returns(true);
    }
}
