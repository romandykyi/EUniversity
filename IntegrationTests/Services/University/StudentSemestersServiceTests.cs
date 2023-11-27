using EUniversity.Core.Dtos.University;
using EUniversity.Core.Models.University;
using EUniversity.Core.Policy;
using EUniversity.Core.Services.University;

namespace EUniversity.IntegrationTests.Services.University;

public class StudentSemestersServiceTests :
    AssigningServiceTests<IStudentSemestersService, StudentSemester, int, StudentSemesterViewDto, int, string>
{
    protected override async Task<int> GetIdOfExistingEntity1Async()
    {
        var semester = SemestersServiceTests.GetTestSemester();
        DbContext.Add(semester);
        await DbContext.SaveChangesAsync();
        return semester.Id;
    }

    protected override async Task<string> GetIdOfExistingEntity2Async()
    {
        var student = await RegisterTestUserAsync(Roles.Student);
        return student.Id;
    }

    protected override StudentSemester GetTestAssigningEntity(int semesterId, string studentId)
    {
        return new StudentSemester()
        {
            SemesterId = semesterId,
            StudentId = studentId
        };
    }
}
