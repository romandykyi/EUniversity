using Duende.IdentityServer.EntityFramework.Options;
using EUniversity.Core.Models;
using EUniversity.Core.Models.University;
using EUniversity.Core.Models.University.Grades;
using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.Extensions.Options;

namespace EUniversity.Infrastructure.Data;

public class ApplicationDbContext : ApiAuthorizationDbContext<ApplicationUser>
{
    public DbSet<Classroom> Classrooms { get; set; }
    public DbSet<Course> Courses { get; set; }
    public DbSet<Class> Classes { get; set; }
    public DbSet<ClassType> ClassTypes { get; set; }
    public DbSet<Group> Groups { get; set; }
    public DbSet<StudentGroup> StudentGroups { get; set; }
    public DbSet<Grade> Grades { get; set; }
    public DbSet<Semester> Semesters { get; set; }
    public DbSet<StudentSemester> StudentSemesters { get; set; }
    public DbSet<ActivityType> ActivityTypes { get; set; }

    public ApplicationDbContext(DbContextOptions options, IOptions<OperationalStoreOptions> operationalStoreOptions)
        : base(options, operationalStoreOptions)
    {

    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // AssignedGrade->Grade
        //builder.Entity<AssignedGrade>()
        //    .HasOne(c => c.Grade)
        //    .WithMany()
        //    .HasForeignKey(c => c.GradeId)
        //    .OnDelete(DeleteBehavior.ClientCascade);

        // ApplicationUser(Student)->Groups
        builder.Entity<ApplicationUser>()
            .HasMany<Group>()
            .WithMany(g => g.Students)
            .UsingEntity<StudentGroup>();

        // ApplicationUser(Student)->Semesters
        builder.Entity<ApplicationUser>()
            .HasMany<Semester>()
            .WithMany(s => s.Students)
            .UsingEntity<StudentSemester>();
    }
}