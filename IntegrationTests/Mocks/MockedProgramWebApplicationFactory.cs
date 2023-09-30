using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http.Headers;

namespace EUniversity.IntegrationTests.Mocks
{
    /// <summary>
    /// <see cref="WebApplicationFactory{}" /> for testing with mocked services and authentication.
    /// </summary>
    public class MockedProgramWebApplicationFactory : WebApplicationFactory<Program>
	{
		public WebApplicationFactory<Program> WithAuthentication(TestClaimsProvider claimsProvider)
		{
			return WithWebHostBuilder(builder =>
			{
				builder.ConfigureTestServices(services =>
				{
					services.AddAuthentication("Test")
							.AddScheme<AuthenticationSchemeOptions, TestAuthHandler>("Test", op => { });

					services.AddScoped(_ => claimsProvider);
				});
			});
		}

		public HttpClient CreateUnauthorizedClient()
		{
			return CreateClient(new()
			{
				AllowAutoRedirect = false
			});
		}

		public HttpClient CreateAuthorizedClient(TestClaimsProvider claimsProvider)
		{
			var client = WithAuthentication(claimsProvider).CreateClient(new()
			{
				AllowAutoRedirect = false
			});

			client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Test");

			return client;
		}
	}
}
