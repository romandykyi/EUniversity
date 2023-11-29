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

    protected override StudentSemester CreateAssigningEntity(int semesterId, string studentId)
    {
        return new StudentSemester()
        {
            SemesterId = semesterId,
            StudentId = studentId,
            EnrollmentDate = DateTimeOffset.Now
        };
    }

    /// <inheritdoc />
    protected override IQueryable<StudentSemester> GetPageQuery(int id1)
    {
        return AssigningEntities
            .Include(sm => sm.Student)
            .AsNoTracking()
            .Where(sm => sm.SemesterId == id1);
    }
}
