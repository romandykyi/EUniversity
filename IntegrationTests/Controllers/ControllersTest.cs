using EUniversity.IntegrationTests.Mocks;
using EUniversity.Core.Policy;
using static EUniversity.IntegrationTests.Mocks.TestClaimsProvider;

namespace EUniversity.IntegrationTests.Controllers
{
	/// <summary>
	/// Base class for integration tests of controllers with mocked services.
	/// </summary>
	public class ControllersTest : IntegrationTest<MockedProgramWebApplicationFactory>
	{
		public HttpClient CreateUnauthorizedClient() =>
			WebApplicationFactory.CreateUnauthorizedClient();
		public HttpClient CreateAdministratorClient() =>
			WebApplicationFactory.CreateAuthorizedClient(Create("Student", Roles.Administrator));
		public HttpClient CreateStudentClient() =>
			WebApplicationFactory.CreateAuthorizedClient(Create("Student", Roles.Student));
		public HttpClient CreateTeacherClient() =>
			WebApplicationFactory.CreateAuthorizedClient(Create("Student", Roles.Teacher));
	}
}
