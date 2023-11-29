using EUniversity.Core.Models.University;
using EUniversity.Core.Services.University;
using EUniversity.Infrastructure.Data;
using System.Linq.Expressions;

namespace EUniversity.Infrastructure.Services.University;

/// <summary>
/// Represents a service which configures
/// the 'Students->Groups' many-to-many relationship.
/// </summary>
public class StudentGroupsService :
    AssigningService<StudentGroup, int, string>, IStudentGroupsService
{
    public StudentGroupsService(ApplicationDbContext dbContext) : base(dbContext)
    {
    }

    public override Expression<Func<StudentGroup, bool>> AssigningEntityPredicate(int groupId, string studentId)
    {
        return sg => sg.StudentId == studentId && sg.GroupId == groupId;
    }

    protected override StudentGroup CreateAssigningEntity(int groupId, string studentId)
    {
        return new()
        {
            GroupId = groupId,
            StudentId = studentId,
            EnrollmentDate = DateTimeOffset.Now
        };
    }

    /// <inheritdoc />
    protected override IQueryable<StudentGroup> GetPageQuery(int id1)
    {
        return AssigningEntities
            .Include(sg => sg.Student)
            .AsNoTracking()
            .Where(sg => sg.GroupId == id1);
    }
}
