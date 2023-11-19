using EUniversity.Core.Models;
using EUniversity.Core.Models.University;
using EUniversity.Core.Services.University;
using EUniversity.Core.Dtos.University;

namespace EUniversity.IntegrationTests.Controllers.University;

public class StudentSemestersEndpointsTests :
    AssigningEndpointsTests<IStudentSemestersService, StudentSemester, Semester, int, ApplicationUser, string>
{
    public override int TestId1 => 7;

    public override string TestId2 => "test-student-id";

    public override string AssignRoute => $"api/semesters/{TestId1}/students";

    public override string UnassignRoute => $"api/semesters/{TestId1}/students/{TestId2}";

    public override void SetUpService()
    {
        ServiceMock = WebApplicationFactory.StudentSemestersServiceMock;
    }

    protected override object GetAssignDto()
    {
        return new AssignStudentDto(TestId2);
    }
}
