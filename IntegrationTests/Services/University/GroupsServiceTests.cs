﻿using EUniversity.Core.Dtos.University;
using EUniversity.Core.Dtos.Users;
using EUniversity.Core.Models;
using EUniversity.Core.Models.University;
using EUniversity.Core.Policy;
using EUniversity.Core.Services.University;
using Mapster;

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
            CollectionAssert.AreEquivalent(expectedResult.Students, group.Students);
        });
    }
}
