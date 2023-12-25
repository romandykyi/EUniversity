using EUniversity.Core.Models.University.Grades;
using EUniversity.Infrastructure.Filters;

namespace EUniversity.Tests.Filters;

public class GradesFilterTests
{
    private readonly Grade[] TestGrades =
    {
        new Grade() {Id = 1, Name = "1", Score = 500},
        new Grade() {Id = 3, Name = "3", Score = 700},
        new Grade() {Id = 2, Name = "2", Score = 600},
        new Grade() {Id = 4, Name = "10", Score = 800},
    };

    [Test]
    public void EmptyFilter_ReturnsEntireQuery()
    {
        // Arrange
        GradesFilterProperties properties = new();
        GradesFilter filter = new(properties);

        // Act
        var result = filter.Apply(TestGrades.AsQueryable());

        // Assert
        Assert.That(result, Is.EquivalentTo(TestGrades));
    }

    [Test]
    public void NameSpecified_ReturnsFilteredQuery()
    {
        // Arrange
        GradesFilterProperties properties = new(Name: "1");
        GradesFilter filter = new(properties);
        int[] expectedIds = { 1, 4 };

        // Act
        var actualIds = filter.Apply(TestGrades.AsQueryable())
            .Select(g => g.Id);

        // Assert
        Assert.That(actualIds, Is.EquivalentTo(expectedIds));
    }

    [Test]
    public void SortByName_ReturnsOrderedByNameQuery()
    {
        // Arrange
        GradesFilterProperties properties = new(SortingMode: GradesSortingMode.Name);
        GradesFilter filter = new(properties);

        // Act
        var result = filter.Apply(TestGrades.AsQueryable());

        // Assert
        Assert.That(result, Is.EquivalentTo(TestGrades));
        Assert.That(result, Is.Ordered.Ascending.By("Name"));
    }

    [Test]
    public void SortByNameDescending_ReturnsOrderedByNameQuery()
    {
        // Arrange
        GradesFilterProperties properties = new(SortingMode: GradesSortingMode.NameDescending);
        GradesFilter filter = new(properties);

        // Act
        var result = filter.Apply(TestGrades.AsQueryable());

        // Assert
        Assert.That(result, Is.EquivalentTo(TestGrades));
        Assert.That(result, Is.Ordered.Descending.By("Name"));
    }

    [Test]
    public void SortByScore_ReturnsOrderedByScoreQuery()
    {
        // Arrange
        GradesFilterProperties properties = new(SortingMode: GradesSortingMode.Score);
        GradesFilter filter = new(properties);

        // Act
        var result = filter.Apply(TestGrades.AsQueryable());

        // Assert
        Assert.That(result, Is.EquivalentTo(TestGrades));
        Assert.That(result, Is.Ordered.Ascending.By("Score"));
    }

    [Test]
    public void SortByScoreDescending_ReturnsOrderedByScoreQuery()
    {
        // Arrange
        GradesFilterProperties properties = new(SortingMode: GradesSortingMode.ScoreDescending);
        GradesFilter filter = new(properties);

        // Act
        var result = filter.Apply(TestGrades.AsQueryable());

        // Assert
        Assert.That(result, Is.EquivalentTo(TestGrades));
        Assert.That(result, Is.Ordered.Descending.By("Score"));
    }
}
