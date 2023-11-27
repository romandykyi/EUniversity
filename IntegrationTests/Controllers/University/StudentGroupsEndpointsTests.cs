using EUniversity.Core.Dtos.University;
using EUniversity.Core.Dtos.Users;
using EUniversity.Core.Models;
using EUniversity.Core.Models.University;
using EUniversity.Core.Services.University;

namespace EUniversity.IntegrationTests.Controllers.University;

public class StudentGroupsEndpointsTests :
    AssigningEndpointsTests<IStudentGroupsService, StudentGroup, Group, int, ApplicationUser, string, StudentGroupViewDto>
{
    public override int TestId1 => 5;

    public override string TestId2 => "test-user-id";

    public override string GetPageRoute => $"api/groups/{TestId1}/students";

    public override string AssignRoute => $"api/groups/{TestId1}/students";

    public override string UnassignRoute => $"api/groups/{TestId1}/students/{TestId2}";

    [SetUp]
    public override void SetUpService()
    {
        ServiceMock = WebApplicationFactory.StudentGroupsServiceMock;
    }

    protected override object GetAssignDto()
    {
        return new AssignStudentDto(TestId2);
    }

    protected override StudentGroupViewDto GetTestPreviewDto()
    {
        StudentPreviewDto student = new("test-id", "test-user", "Test", "User", null);
        return new(student, DateTimeOffset.Now);
    }
}
