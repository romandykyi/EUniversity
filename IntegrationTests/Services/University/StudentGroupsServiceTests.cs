using EUniversity.Core.Dtos.University;
using EUniversity.Core.Models.University;
using EUniversity.Core.Policy;
using EUniversity.Core.Services.University;

namespace EUniversity.IntegrationTests.Services.University;

public class StudentGroupsServiceTests :
    AssigningServiceTests<IStudentGroupsService, StudentGroup, int, StudentGroupViewDto, int, string>
{
    protected override async Task<int> GetIdOfExistingEntity1Async()
    {
        var teacher = await RegisterTestUserAsync();
        var course = CoursesServiceTests.CreateTestCourse();
        DbContext.Add(course);
        await DbContext.SaveChangesAsync();

        var group = GroupsServiceTests.GetTestGroup(course, teacher);
        DbContext.Add(group);
        await DbContext.SaveChangesAsync();
        return group.Id;
    }

    protected override async Task<string> GetIdOfExistingEntity2Async()
    {
        var student = await RegisterTestUserAsync(Roles.Student);
        return student.Id;
    }

    protected override StudentGroup GetTestAssigningEntity(int groupId, string studentId)
    {
        return new()
        {
            GroupId = groupId,
            StudentId = studentId
        };
    }
}
