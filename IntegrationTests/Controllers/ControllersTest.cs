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

		private HttpClient CreateAuthorizedClient(string? id = null)
		{
			WebApplicationFactory.ClaimsProvider.Init(id, "Admin", Roles.Administrator);
			return WebApplicationFactory.CreateAuthorizedClient();
		}

		public HttpClient CreateAdministratorClient(string? id = null) =>
			CreateAuthorizedClient(id);
		public HttpClient CreateStudentClient(string? id = null) =>
			CreateAuthorizedClient(id);
		public HttpClient CreateTeacherClient(string? id = null) =>
			CreateAuthorizedClient(id);
	}
}
