using EUniversity.Core.Models;
using EUniversity.Infrastructure.Filters;
using IdentityModel;

namespace EUniversity.Tests.Filters;

public class DefaultFilterTests
{
    private class TestEntity : IHasName, IHasCreationDate, IEquatable<TestEntity>
    {
        public string Name { get; set; }

        public DateTimeOffset CreationDate { get; set; }

        public TestEntity(string name, DateTimeOffset creationDate)
        {
            Name = name;
            CreationDate = creationDate;
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as TestEntity);
        }

        public bool Equals(TestEntity? entity)
        {
            return entity is not null && Name == entity.Name &&
                   CreationDate == entity.CreationDate;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Name, CreationDate);
        }

        public static bool operator ==(TestEntity left, TestEntity right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(TestEntity left, TestEntity right)
        {
            return !(left == right);
        }
    }

    [Test]
    public void EmptyFilter_ReturnsEntireQuery()
    {
        // Arrange
        DefaultFilter<TestEntity> filter = new(string.Empty, DefaultFilterSortingMode.Default);
        TestEntity[] array =
        {
            new("Cucumber",DateTimeOffset.MinValue),
            new("Apple",DateTimeOffset.MinValue),
            new("Tomato",DateTimeOffset.MinValue),
            new("Banana",DateTimeOffset.MinValue)
        };

        // Act
        var result = filter.Apply(array.AsQueryable());

        // Assert
        Assert.That(result, Is.EquivalentTo(array));
    }

    [Test]
    public void FilterSpecified_ReturnsFilteredQuery()
    {
        // Arrange
        DefaultFilter<TestEntity> filter = new("berry", DefaultFilterSortingMode.Default);
        TestEntity[] sourceArray =
        {
            new("Blueberry",DateTimeOffset.MinValue),
            new("Grape",DateTimeOffset.MinValue),
            new("Blackberry",DateTimeOffset.MinValue),
            new("Cherry",DateTimeOffset.MinValue),
            new("Strawberry",DateTimeOffset.MinValue)
        };
        TestEntity[] expectedArray =
        {
            new("Blueberry",DateTimeOffset.MinValue),
            new("Strawberry",DateTimeOffset.MinValue),
            new("Blackberry",DateTimeOffset.MinValue)
        };

        // Act
        var result = filter.Apply(sourceArray.AsQueryable());

        // Assert
        Assert.That(result, Is.EquivalentTo(expectedArray));
    }

    [Test]
    public void FilterSpecified_NoMatches_ReturnsEmptyQuery()
    {
        // Arrange
        DefaultFilter<TestEntity> filter = new("Orange", DefaultFilterSortingMode.Default);
        TestEntity[] array =
        {
            new("Cucumber",DateTimeOffset.MinValue),
            new("Apple",DateTimeOffset.MinValue),
            new("Tomato",DateTimeOffset.MinValue),
            new("Banana",DateTimeOffset.MinValue)
        };

        // Act
        var result = filter.Apply(array.AsQueryable());

        // Assert
        Assert.That(result, Is.Empty);
    }

    private readonly TestEntity[] testArray =
        {
            new("John", new(0, TimeSpan.Zero)),
            new("Alice", new(2, TimeSpan.Zero)),
            new("Wilson", new(4, TimeSpan.Zero)),
            new("Bob", new(3, TimeSpan.Zero))
        };

    [Test]
    public void SortByName_ReturnsOrderedByNameQuery()
    {
        // Arrange
        DefaultFilter<TestEntity> filter = new(string.Empty, DefaultFilterSortingMode.Name);

        // Act
        var result = filter.Apply(testArray.AsQueryable());

        // Assert
        Assert.That(result, Is.Ordered.Ascending.By("Name"));
    }

    [Test]
    public void SortByNameDescending_ReturnsOrderedByNameInDescendingOrderQuery()
    {
        // Arrange
        DefaultFilter<TestEntity> filter = new(string.Empty, DefaultFilterSortingMode.NameDescending);

        // Act
        var result = filter.Apply(testArray.AsQueryable());

        // Assert
        Assert.That(result, Is.Ordered.Descending.By("Name"));
    }

    [Test]
    public void NewestSortingMode_ReturnsOrderedByCreationDateInDescendingOrderQuery()
    {
        // Arrange
        DefaultFilter<TestEntity> filter = new(string.Empty, DefaultFilterSortingMode.Newest);

        // Act
        var result = filter.Apply(testArray.AsQueryable());

        // Assert
        Assert.That(result, Is.Ordered.Descending.By("CreationDate"));
    }

    [Test]
    public void OldestSortingMode_ReturnsOrderedByCreationDateQuery()
    {
        // Arrange
        DefaultFilter<TestEntity> filter = new(string.Empty, DefaultFilterSortingMode.Oldest);

        // Act
        var result = filter.Apply(testArray.AsQueryable());

        // Assert
        Assert.That(result, Is.Ordered.Ascending.By("CreationDate"));
    }
}
