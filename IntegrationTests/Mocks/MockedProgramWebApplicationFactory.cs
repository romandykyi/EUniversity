using EUniversity.Core.Models;
using EUniversity.Core.Services;
using EUniversity.Extensions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
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
        public TestClaimsProvider ClaimsProvider { get; private set; } = null!;
        public IAuthService AuthServiceMock { get; private set; } = null!;
        public UserManager<ApplicationUser> UserManagerMock { get; private set; } = null!;

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            ClaimsProvider = new();
            AuthServiceMock = Substitute.For<IAuthService>();

            var mockedUserStore = Substitute.For<IUserStore<ApplicationUser>>();
            UserManagerMock = Substitute.For<UserManager<ApplicationUser>>(
                mockedUserStore, null, null, null, null, null, null, null, null
                );

            builder.ConfigureTestServices(services =>
            {
                services.AddScoped(_ => ClaimsProvider);

                services.AddAuthentication(defaultScheme: "TestScheme")
                    .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>(
                        "TestScheme", options => { });
                services.AddCustomizedAuthorization("TestScheme");

                services.AddScoped(_ => AuthServiceMock);
                services.AddScoped(_ => UserManagerMock);
            });
        }

        public HttpClient CreateCustomClient()
        {
            var client = CreateClient(new()
            {
                AllowAutoRedirect = false
            });

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("TestScheme");

            return client;
        }
    }
}
