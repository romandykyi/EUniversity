using EUniversity.Core.Dtos.University;
using EUniversity.Core.Models;
using EUniversity.Core.Models.University;
using EUniversity.Core.Policy;
using EUniversity.Core.Services.University;

namespace EUniversity.IntegrationTests.Services.University;

public class GroupsServiceTests :
    CrudServicesTest<IGroupsService, Group, int, GroupPreviewDto, GroupViewDto, GroupCreateDto, GroupCreateDto>
{
    private Course _testCourse;
    private ApplicationUser _testTeacher;

    /// <inheritdoc />
    protected override void AssertThatWasUpdated(Group actualEntity, GroupCreateDto updateDto)
    {
        Assert.Multiple(() =>
        {
            Assert.That(actualEntity.Name, Is.EqualTo(updateDto.Name));
            Assert.That(actualEntity.TeacherId, Is.EqualTo(updateDto.TeacherId));
            Assert.That(actualEntity.CourseId, Is.EqualTo(updateDto.CourseId));
        });
    }

    /// <inheritdoc />
    protected override int GetNonExistentId()
    {
        return -1;
    }

    /// <inheritdoc />
    protected override Group GetTestEntity()
    {
        return new Group()
        {
            Name = "100-A",
            CourseId = _testCourse.Id,
            TeacherId = _testTeacher.Id
        };
    }

    /// <inheritdoc />
    protected override GroupCreateDto GetValidCreateDto()
    {
        return new("112-A", _testCourse.Id, _testTeacher.Id);
    }

    /// <inheritdoc />
    protected override GroupCreateDto GetValidUpdateDto()
    {
        return new("100-B", _testCourse.Id, null);
    }

    [SetUp]
    public async Task SetUpDependencies()
    {
        _testCourse = CoursesServiceTests.CreateTestCourse();
        DbContext.Add(_testCourse);
        await DbContext.SaveChangesAsync();

        _testTeacher = await RegisterTestUser(Roles.Teacher);
    }
}
