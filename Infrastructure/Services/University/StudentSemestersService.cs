using EUniversity.Core.Models.University;
using EUniversity.Core.Services.University;
using EUniversity.Infrastructure.Data;
using System.Linq.Expressions;

namespace EUniversity.Infrastructure.Services.University;

public class StudentSemestersService :
    AssigningService<StudentSemester, int, string>, IStudentSemestersService
{
    public StudentSemestersService(ApplicationDbContext dbContext) : base(dbContext)
    {
    }

    public override Expression<Func<StudentSemester, bool>> AssigningEntityPredicate(int semesterId, string studentId)
    {
        return ss => ss.SemesterId == semesterId && ss.StudentId == studentId;
    }

    public override StudentSemester CreateAssigningEntity(int semesterId, string studentId)
    {
        return new StudentSemester()
        {
            SemesterId = semesterId,
            StudentId = studentId
        };
    }
}
