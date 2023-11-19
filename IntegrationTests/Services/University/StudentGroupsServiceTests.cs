using EUniversity.Core.Models.University;
using EUniversity.Core.Policy;
using EUniversity.Core.Services.University;

namespace EUniversity.IntegrationTests.Services.University;

public class StudentGroupsServiceTests :
    AssigningServiceTests<IStudentGroupsService, StudentGroup, string, int>
{
    protected override async Task<string> GetIdOfExistingEntity1Async()
    {
        var student = await RegisterTestUserAsync(Roles.Student);
        return student.Id;
    }

    protected override async Task<int> GetIdOfExistingEntity2Async()
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

    protected override StudentGroup GetTestAssigningEntity(string studentId, int groupId)
    {
        return new()
        {
            StudentId = studentId,
            GroupId = groupId
        };
    }
}
