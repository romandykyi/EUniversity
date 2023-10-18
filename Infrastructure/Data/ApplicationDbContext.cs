using Duende.IdentityServer.EntityFramework.Options;
using EUniversity.Core.Models;
using EUniversity.Core.Models.University;
using EUniversity.Core.Models.University.Grades;
using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.Extensions.Options;

namespace EUniversity.Infrastructure.Data
{
    public class ApplicationDbContext : ApiAuthorizationDbContext<ApplicationUser>
    {
        public DbSet<Classroom> Classrooms { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<Grade> Grades { get; set; }
        public DbSet<CourseGrade> CourseGrades { get; set; }

        public ApplicationDbContext(DbContextOptions options, IOptions<OperationalStoreOptions> operationalStoreOptions)
            : base(options, operationalStoreOptions)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // CourseGrade->Grade
            builder.Entity<CourseGrade>()
                .HasOne(c => c.Grade)
                .WithMany(g => g.CourseGrades)
                .HasForeignKey(c => c.GradeId)
                .OnDelete(DeleteBehavior.ClientCascade);
        }
    }
}