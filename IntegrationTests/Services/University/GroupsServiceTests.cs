using EUniversity.Core.Dtos.University;
using EUniversity.Core.Models;
using EUniversity.Core.Models.University;
using EUniversity.Core.Policy;
using EUniversity.Core.Services.University;
using Mapster;
using Microsoft.EntityFrameworkCore;

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
            Course = _testCourse,
            TeacherId = _testTeacher.Id,
            Teacher = _testTeacher,
            Students = new List<ApplicationUser>()
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

    [Test]
    public override async Task GetById_ElementExists_ReturnsValidElement()
    {
        // Arrange
        var group = await CreateTestEntityAsync();
        var expectedResult = group.Adapt<GroupViewDto>();

        // Act
        var result = await Service.GetByIdAsync(group.Id);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(result.Id, Is.EqualTo(expectedResult.Id));
            Assert.That(result.Name, Is.EqualTo(expectedResult.Name));
            Assert.That(result.Teacher, Is.EqualTo(expectedResult.Teacher));
            Assert.That(result.Course, Is.EqualTo(expectedResult.Course));
            Assert.That(result.Students, Is.EquivalentTo(expectedResult.Students));
        });
    }

    // Creates a test group, a test student and assigns the student to the group
    private async Task<StudentGroup> CreateTestStudentGroupAsync()
    {
        var group = await CreateTestEntityAsync();
        var student = await RegisterTestUser(Roles.Student);
        StudentGroup studentGroup = new()
        {
            GroupId = group.Id,
            StudentId = student.Id
        };
        DbContext.Add(studentGroup);
        await DbContext.SaveChangesAsync();
        return studentGroup;
    }

    // Checks exactly one StudentGroup exists
    private async Task<bool> CheckStudentGroupExistenceAsync(string studentId, int groupId)
    {
        return await DbContext.StudentGroups.SingleOrDefaultAsync(
                sg => sg.StudentId == studentId && sg.GroupId == groupId
            ) != null;
    }

    [Test]
    public virtual async Task AddStudent_StudentIsNotInGroup_AddsStudentAndReturnsTrue()
    {
        // Arrange
        var student = await RegisterTestUser(Roles.Student);
        var group = await CreateTestEntityAsync();
        StudentGroupDto dto = new(student.Id, group.Id);

        // Act
        bool result = await Service.AddStudentAsync(dto);

        // Assert
        Assert.That(result, Is.True);
        // Assert that student is added to the group
        bool exists = await CheckStudentGroupExistenceAsync(student.Id, group.Id);
        Assert.That(exists, "Student wasn't added to the group");
    }

    [Test]
    public virtual async Task AddStudent_StudentIsAlreadyInGroup_ReturnsFalse()
    {
        // Arrange
        var studentGroup = await CreateTestStudentGroupAsync();
        StudentGroupDto dto = new(studentGroup.StudentId, studentGroup.GroupId);

        // Act
        bool result = await Service.AddStudentAsync(dto);

        // Assert
        Assert.That(result, Is.False);
    }

    [Test]
    public virtual async Task RemoveStudent_StudentIsInGroup_RemovesStudentAndReturnsTrue()
    {
        // Arrange
        var studentGroup = await CreateTestStudentGroupAsync();
        StudentGroupDto dto = new(studentGroup.StudentId, studentGroup.GroupId);

        // Act
        bool result = await Service.RemoveStudentAsync(dto);

        // Assert
        Assert.That(result, Is.True);
        // Assert that student was removed
        bool exists = await CheckStudentGroupExistenceAsync(dto.StudentId, dto.GroupId);
        Assert.That(exists, Is.False, "Student wasn't removed from the group");
    }

    [Test]
    public virtual async Task RemoveStudent_StudentIsNotInGroup_ReturnsFalse()
    {
        // Arrange
        var student = await RegisterTestUser(Roles.Student);
        var group = await CreateTestEntityAsync();
        StudentGroupDto dto = new(student.Id, group.Id);

        // Act
        bool result = await Service.RemoveStudentAsync(dto);

        // Assert
        Assert.That(result, Is.False);
    }
}
