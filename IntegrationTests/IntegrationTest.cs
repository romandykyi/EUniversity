using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;

namespace EUniversity.IntegrationTests
{
    /// <summary>
    /// Base class for integration tests.
    /// </summary>
    public abstract class IntegrationTest<TWebApplicationFactory>
        where TWebApplicationFactory : WebApplicationFactory<Program>, new()
    {
        protected TWebApplicationFactory WebApplicationFactory { get; private set; }
        protected IServiceScopeFactory ScopeFactory { get; private set; }
        protected IServiceScope ServiceScope { get; private set; }

        [SetUp]
        public void SetUpIntegration()
        {
            WebApplicationFactory = Activator.CreateInstance<TWebApplicationFactory>();
            WebApplicationFactory.Server.PreserveExecutionContext = true;

            ScopeFactory = WebApplicationFactory.Services.GetService<IServiceScopeFactory>()!;
            ServiceScope = ScopeFactory.CreateScope();
        }

        [TearDown]
        public void TearDownIntegration()
        {
            ServiceScope.Dispose();
            WebApplicationFactory.Dispose();
        }
    }
}
