using EUniversity.Core.Filters;
using EUniversity.Core.Models;

namespace EUniversity.Tests.Filters;

public class NameFilterTests
{
    private record NamedEntity(string Name) : IHasName;

    [Test]
    public void EmptyFilter_ReturnsEntireQuery()
    {
        // Arrange
        NameFilter<NamedEntity> filter = new(string.Empty);
        NamedEntity[] array = { new("Cucumber"), new("Apple"), new("Tomato"), new("Banana") };

        // Act
        var result = filter.Apply(array.AsQueryable());

        // Assert
        Assert.That(result, Is.EquivalentTo(array));
    }

    [Test]
    public void FilterSpecified_ReturnsFilteredQuery()
    {
        // Arrange
        NameFilter<NamedEntity> filter = new("berry");
        NamedEntity[] sourceArray = { new("Blueberry"), new("Grape"), new("Blackberry"), new("Cherry"), new("Strawberry") };
        NamedEntity[] expectedArray = { new("Blueberry"), new("Strawberry"), new("Blackberry") };

        // Act
        var result = filter.Apply(sourceArray.AsQueryable());

        // Assert
        Assert.That(result, Is.EquivalentTo(expectedArray));
    }

    [Test]
    public void FilterSpecified_NoMatches_ReturnsEmptyQuery()
    {
        // Arrange
        NameFilter<NamedEntity> filter = new("Orange");
        NamedEntity[] array = { new("Cucumber"), new("Apple"), new("Tomato"), new("Banana") };

        // Act
        var result = filter.Apply(array.AsQueryable());

        // Assert
        Assert.That(result, Is.Empty);
    }
}
