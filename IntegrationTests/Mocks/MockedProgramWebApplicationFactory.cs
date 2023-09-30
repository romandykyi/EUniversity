using EUniversity.Core.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using System.Net.Http.Headers;

namespace EUniversity.IntegrationTests.Mocks
{
	/// <summary>
	/// <see cref="WebApplicationFactory{}" /> for testing with mocked services and authentication.
	/// </summary>
	public class MockedProgramWebApplicationFactory : WebApplicationFactory<Program>
	{
		public IAuthService AuthServiceMock { get; private set; } = null!;

		protected override void ConfigureWebHost(IWebHostBuilder builder)
		{
			AuthServiceMock = Substitute.For<IAuthService>();

			builder.ConfigureTestServices(services =>
			{
				services.AddScoped(_ => AuthServiceMock);
			});
		}

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
