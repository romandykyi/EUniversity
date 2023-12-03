using EUniversity.Core.Dtos.University;
using EUniversity.Core.Models;
using EUniversity.Core.Models.University;
using EUniversity.Core.Policy;
using EUniversity.Core.Services.University;

namespace EUniversity.IntegrationTests.Services.University;

public class ClassesServiceTests :
    CrudServicesTest<IClassesService, Class, int, ClassViewDto, ClassViewDto, ClassCreateDto, ClassUpdateDto>
{
    private Classroom _testClassroom;
    private Group _testGroup;
    private ApplicationUser _testSubstituteTeacher;

    /// <inheritdoc />
    protected override void AssertThatWasUpdated(Class actualEntity, ClassUpdateDto updateDto)
    {
        Assert.Multiple(() =>
        {
            Assert.That(actualEntity.ClassroomId, Is.EqualTo(updateDto.ClassroomId));
            Assert.That(actualEntity.GroupId, Is.EqualTo(updateDto.GroupId));
            Assert.That(actualEntity.SubstituteTeacherId, Is.EqualTo(updateDto.SubstituteTeacherId));
            Assert.That(actualEntity.Duration, Is.EqualTo(updateDto.Duration));
            Assert.That(actualEntity.StartDate, Is.EqualTo(updateDto.StartDate));
        });
    }

    /// <inheritdoc />
    protected override int GetNonExistentId()
    {
        return -1;
    }

    /// <inheritdoc />
    protected override Class GetTestEntity()
    {
        return new()
        {
            Duration = TimeSpan.FromHours(1),
            ClassroomId = _testClassroom.Id,
            GroupId = _testGroup.Id,
            SubstituteTeacherId = _testSubstituteTeacher.Id,
            StartDate = DateTimeOffset.Now
        };
    }

    /// <inheritdoc />
    protected override ClassCreateDto GetValidCreateDto()
    {
        return new(_testClassroom.Id, _testGroup.Id, _testSubstituteTeacher.Id,
            DateTimeOffset.Now, TimeSpan.FromHours(1));
    }

    /// <inheritdoc />
    protected override ClassUpdateDto GetValidUpdateDto()
    {
        return new(_testClassroom.Id, _testGroup.Id, null,
            DateTimeOffset.Now, TimeSpan.FromHours(1));
    }

    [SetUp]
    public async Task SetUpDependencies()
    {
        _testClassroom = ClassroomsServiceTests.CreateTestClassroom();
        DbContext.Add(_testClassroom);
        await DbContext.SaveChangesAsync();

        var course = CoursesServiceTests.CreateTestCourse();
        DbContext.Add(course);
        await DbContext.SaveChangesAsync();

        var teacher = await RegisterTestUserAsync(Roles.Teacher);

        _testGroup = GroupsServiceTests.GetTestGroup(course, teacher);
        DbContext.Add(_testGroup);
        await DbContext.SaveChangesAsync();

        _testSubstituteTeacher = await RegisterTestUserAsync(Roles.Teacher);
    }
}
