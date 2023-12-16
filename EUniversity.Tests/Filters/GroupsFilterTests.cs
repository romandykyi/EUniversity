using EUniversity.Core.Models;
using EUniversity.Core.Models.University;
using EUniversity.Infrastructure.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EUniversity.Tests.Filters;

public class GroupsFilterTests
{
    private static readonly Semester TestSemester = new()
    {
        Id = 100
    };

    private static readonly ApplicationUser TestTeacher1 = new()
    {
        Id = "test-teacher-id1"
    };
    private static readonly ApplicationUser TestTeacher2 = new()
    {
        Id = "test-teacher-id2"
    };

    private static readonly Course CourseWithSemester = new()
    {
        Id = 200,
        Semester = TestSemester,
        SemesterId = TestSemester.Id
    };
    private static readonly Course NoSemesterCourse = new()
    {
        Id = 201
    };

    private static readonly Group[] TestGroups =
    {
        new Group() {Id = 300, Name = "Group I", Course = CourseWithSemester, CourseId = CourseWithSemester.Id, Teacher = TestTeacher2, TeacherId = TestTeacher2.Id},
        new Group() {Id = 301, Name = "Group II", Course = NoSemesterCourse, CourseId = NoSemesterCourse.Id, Teacher = TestTeacher2, TeacherId = TestTeacher2.Id},
        new Group() {Id = 302, Name = "Group III", Course = CourseWithSemester, CourseId = CourseWithSemester.Id, Teacher = TestTeacher1, TeacherId = TestTeacher1.Id},
        new Group() {Id = 303, Name = "Group IV", Course = NoSemesterCourse, CourseId = NoSemesterCourse.Id, Teacher = TestTeacher1, TeacherId = TestTeacher1.Id}
    };

    [Test]
    public void EmptyFilter_ReturnsEntireQuery()
    {
        // Arrange
        GroupsFilterProperties properties = new();
        GroupsFilter filter = new(properties, string.Empty);

        // Act
        var result = filter.Apply(TestGroups.AsQueryable());

        // Assert
        Assert.That(result, Is.EquivalentTo(TestGroups));
    }

    [Test]
    public void TeacherIdSpecified_ReturnsFilteredQuery()
    {
        // Arrange
        GroupsFilterProperties properties = new(TeacherId: TestTeacher1.Id);
        GroupsFilter filter = new(properties, string.Empty);
        int[] expectedIds = { 302, 303 };

        // Act
        var actualIds = filter.Apply(TestGroups.AsQueryable())
            .Select(g => g.Id);

        // Assert
        Assert.That(actualIds, Is.EquivalentTo(expectedIds));
    }

    [Test]
    public void CourseIdSpecified_ReturnsFilteredQuery()
    {
        // Arrange
        GroupsFilterProperties properties = new(CourseId: CourseWithSemester.Id);
        GroupsFilter filter = new(properties, string.Empty);
        int[] expectedIds = { 300, 302 };

        // Act
        var query = TestGroups.AsQueryable();
        var actualIds = filter.Apply(query)
            .Select(g => g.Id);

        // Assert
        Assert.That(actualIds, Is.EquivalentTo(expectedIds));
    }

    [Test]
    public void SemesterIdSpecified_ReturnsFilteredQuery()
    {
        // Arrange
        GroupsFilterProperties properties = new(SemesterId: TestSemester.Id);
        GroupsFilter filter = new(properties, string.Empty);
        int[] expectedIds = { 300, 302 };

        // Act
        var actualIds = filter.Apply(TestGroups.AsQueryable())
            .Select(g => g.Id);

        // Assert
        Assert.That(actualIds, Is.EquivalentTo(expectedIds));
    }

    [Test]
    public void ZeroSemesterId_ReturnsGroupsWithoutSemesters()
    {
        // Arrange
        GroupsFilterProperties properties = new(SemesterId: 0);
        GroupsFilter filter = new(properties, string.Empty);
        int[] expectedIds = { 301, 303 };

        // Act
        var actualIds = filter.Apply(TestGroups.AsQueryable())
            .Select(g => g.Id);

        // Assert
        Assert.That(actualIds, Is.EquivalentTo(expectedIds));
    }
}
