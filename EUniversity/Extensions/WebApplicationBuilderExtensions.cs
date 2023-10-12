using EUniversity.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace EUniversity.Extensions
{
    public static class WebApplicationBuilderExtensions
    {
        public static void ConnectDatabase(this WebApplicationBuilder builder)
        {
            var useInMemoryDb = Environment.GetEnvironmentVariable("USE_IN_MEMORY_DATABASE");
            Action<DbContextOptionsBuilder> dbContextOptions;
            if (useInMemoryDb == null || useInMemoryDb == "false")
            {
                // Use SQL Server
                var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
                dbContextOptions = o => o.UseSqlServer(connectionString, b => b.MigrationsAssembly("EUniversity.Infrastructure"));
            }
            else if (useInMemoryDb == "true")
            {
                // Use in-memory database
                dbContextOptions = o => o.UseInMemoryDatabase("EUniversityTestDb")
                    .ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning));

                logger.LogWarning("In-memory database is used");
            }
            else
            {
                throw new InvalidOperationException("USE_IN_MEMORY_DATABASE value is invalid");
            }

            builder.Services.AddDbContext<ApplicationDbContext>(dbContextOptions);
        }
    }
}
