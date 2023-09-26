using EUniversity.Infrastructure.Data;
using EUniversity.IntegrationTests.Controllers;
using Microsoft.Extensions.DependencyInjection;

namespace EUniversity.IntegrationTests
{
	/// <summary>
	/// Base class for integration tests.
	/// </summary>
	/// <remarks>
	/// Each test in derived classes is isolated, and no changes to the database will be saved.
	/// </remarks>
	public abstract class IntegrationTest
	{
		protected ProgramWebApplicationFactory WebApplicationFactory { get; private set; }
		protected HttpClient Client { get; private set; }
		protected IServiceScopeFactory ScopeFactory { get; private set; }
		protected IServiceScope ServiceScope { get; private set; }
		protected ApplicationDbContext DbContext { get; private set; }

		[SetUp]
		public void SetUpIntegration()
		{
			WebApplicationFactory = new ProgramWebApplicationFactory();
			WebApplicationFactory.Server.PreserveExecutionContext = true;
			Client = WebApplicationFactory.CreateClient();

			ScopeFactory = WebApplicationFactory.Services.GetService<IServiceScopeFactory>()!;
			ServiceScope = ScopeFactory.CreateScope();
			DbContext = ServiceScope.ServiceProvider.GetService<ApplicationDbContext>()!;

			// Begin transaction to make tests isolated
			DbContext.Database.BeginTransaction();
		}

		[TearDown]
		public void TearDownIntegration()
		{
			// Undo all changes
			DbContext.ChangeTracker.Clear();

			ServiceScope.Dispose();
			Client.Dispose();
			WebApplicationFactory.Dispose();
		}
	}
}
