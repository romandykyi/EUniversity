using EUniversity.Core.Policy;
using EUniversity.IntegrationTests.Mocks;

namespace EUniversity.IntegrationTests.Controllers
{
	/// <summary>
	/// Base class for integration tests of controllers with mocked services.
	/// </summary>
	public class ControllersTest : IntegrationTest<MockedProgramWebApplicationFactory>
	{
		public HttpClient CreateUnauthorizedClient() =>
			WebApplicationFactory.CreateUnauthorizedClient();

		public HttpClient CreateAuthorizedClient(string? id, string userName, params string[] roles)
		{
			WebApplicationFactory.ClaimsProvider.Init(id, userName, roles);
			return WebApplicationFactory.CreateAuthorizedClient();
		}

		public HttpClient CreateAdministratorClient(string? id = null) =>
			CreateAuthorizedClient(id, "admin", Roles.Administrator);
		public HttpClient CreateStudentClient(string? id = null) =>
			CreateAuthorizedClient(id, "student", Roles.Student);
		public HttpClient CreateTeacherClient(string? id = null) =>
			CreateAuthorizedClient(id, "teacher", Roles.Teacher);
	}
}
