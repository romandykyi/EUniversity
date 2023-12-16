using EUniversity.Core.Models.University;
using EUniversity.Infrastructure.Filters;

namespace EUniversity.Tests.Filters;

public class CoursesFilterTests
{
    private static readonly Semester TestSemester = new()
    {
        Id = 100
    };

    private static readonly Course[] TestCourses =
    {
        new()
        {
            Id = 200,
            Name = "Math",
            Semester = TestSemester,
            SemesterId = TestSemester.Id
        },
        new()
        {
            Id = 201,
            Name = "Psychology"
        }
    };

    [Test]
    public void EmptyFilter_ReturnsEntireQuery()
    {
        // Arrange
        CoursesFilterProperties properties = new();
        CoursesFilter filter = new(properties, string.Empty);

        // Act
        var result = filter.Apply(TestCourses.AsQueryable());

        // Assert
        Assert.That(result, Is.EquivalentTo(TestCourses));
    }

    [Test]
    public void SemesterIdSpecified_ReturnsFilteredQuery()
    {
        // Arrange
        CoursesFilterProperties properties = new(SemesterId: TestSemester.Id);
        CoursesFilter filter = new(properties, string.Empty);
        int[] expectedIds = { 200 };

        // Act
        var actualIds = filter.Apply(TestCourses.AsQueryable())
            .Select(g => g.Id);

        // Assert
        Assert.That(actualIds, Is.EquivalentTo(expectedIds));
    }

    [Test]
    public void ZeroSemesterId_ReturnsGroupsWithoutSemesters()
    {
        // Arrange
        CoursesFilterProperties properties = new(SemesterId: 0);
        CoursesFilter filter = new(properties, string.Empty);
        int[] expectedIds = { 201 };

        // Act
        var actualIds = filter.Apply(TestCourses.AsQueryable())
            .Select(g => g.Id);

        // Assert
        Assert.That(actualIds, Is.EquivalentTo(expectedIds));
    }
}
