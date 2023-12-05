using EUniversity.Core.Dtos.University;
using EUniversity.Core.Models;
using EUniversity.Core.Models.University;
using EUniversity.Core.Policy;
using EUniversity.Core.Services.University;
using IdentityModel;

namespace EUniversity.IntegrationTests.Services.University;

public class ClassesServiceTests :
    CrudServicesTest<IClassesService, Class, int, ClassViewDto, ClassViewDto, ClassCreateDto, ClassUpdateDto>
{
    private ClassType _testClassType;
    private Classroom _testClassroom;
    private Group _testGroup;
    private ApplicationUser _testSubstituteTeacher;

    /// <inheritdoc />
    protected override void AssertThatWasUpdated(Class actualEntity, ClassUpdateDto updateDto)
    {
        Assert.Multiple(() =>
        {
            Assert.That(actualEntity.ClassTypeId, Is.EqualTo(updateDto.ClassTypeId));
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
            ClassTypeId = _testClassType.Id,
            ClassroomId = _testClassroom.Id,
            GroupId = _testGroup.Id,
            SubstituteTeacherId = _testSubstituteTeacher.Id,
            StartDate = DateTimeOffset.Now
        };
    }

    /// <inheritdoc />
    protected override ClassCreateDto GetValidCreateDto()
    {
        return new(_testClassType.Id, _testClassroom.Id, _testGroup.Id,
            _testSubstituteTeacher.Id,  DateTimeOffset.Now,
            TimeSpan.FromHours(1), null, null);
    }

    /// <inheritdoc />
    protected override ClassUpdateDto GetValidUpdateDto()
    {
        return new(_testClassType.Id, _testClassroom.Id, _testGroup.Id,
            _testSubstituteTeacher.Id, DateTimeOffset.Now,
            TimeSpan.FromHours(1));
    }

    [SetUp]
    public async Task SetUpDependencies()
    {
        _testClassType = new() { Name = "Test" };
        DbContext.Add(_testClassType);
        await DbContext.SaveChangesAsync();

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

    [Test]
    public virtual async Task Create_RepeatsSpecified_CreatesManyClasses()
    {
        // Arrange
        const int repeats = 3;
        const int repeatsDelayDays = 7;
        TimeSpan duration = TimeSpan.FromHours(1);
        DateTimeOffset startDate = DateTimeOffset.Now;
        ClassCreateDto dto = new(_testClassType.Id, 
            _testClassroom.Id, _testGroup.Id, _testSubstituteTeacher.Id,
            startDate, duration, repeats, repeatsDelayDays);

        // Act
        Class @class = await Service.CreateAsync(dto);

        // Assert
        // Created classes
        var classes = DbContext.Classes
            .Where(c => c.ClassroomId == _testClassroom.Id && 
                c.GroupId == _testGroup.Id &&
                c.SubstituteTeacherId == _testSubstituteTeacher.Id &&
                c.Duration == duration)
            .OrderBy(c => c.StartDate)
            .ToArray();

        Assert.That(classes, Has.Length.EqualTo(repeats));
        for (int i = 1; i < classes.Length; i++) 
        {
            DateTimeOffset expectedDate = classes[i].StartDate.AddDays(-repeatsDelayDays * i);
            Assert.That(expectedDate, Is.EqualTo(startDate));
        }
    }
}
