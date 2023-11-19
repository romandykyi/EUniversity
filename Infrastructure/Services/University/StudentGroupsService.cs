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
    AssigningService<StudentGroup, string, int>, IStudentGroupsService
{
    public StudentGroupsService(ApplicationDbContext dbContext) : base(dbContext)
    {
    }

    public override Expression<Func<StudentGroup, bool>> AssigningEntityPredicate(string studentId, int groupId)
    {
        return sg => sg.StudentId == studentId && sg.GroupId == groupId;
    }

    public override StudentGroup CreateAssigningEntity(string studentId, int groupId)
    {
        return new()
        {
            StudentId = studentId,
            GroupId = groupId
        };
    }
}
