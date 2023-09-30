using EUniversity.Infrastructure.Data;
using Microsoft.Extensions.DependencyInjection;

namespace EUniversity.IntegrationTests.Services
{
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
        public void SetUpTransaction()
		{
			DbContext = ServiceScope.ServiceProvider.GetService<ApplicationDbContext>()!;
			// Begin transaction to make tests isolated
			DbContext.Database.BeginTransactionAsync();
        }

        [TearDown]
        public void TearDownTransaction()
        {
            // Undo all changes
            DbContext.ChangeTracker.Clear();
            DbContext.Database.RollbackTransaction();
        }
    }
}
