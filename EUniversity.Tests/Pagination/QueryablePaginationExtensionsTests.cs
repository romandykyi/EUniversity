using EUniversity.Core.Pagination;
using EUniversity.Infrastructure.Pagination;

namespace EUniversity.Tests.Pagination;

public class QueryablePaginationExtensionsTests
{
    [Test]
    public void ApplyPagination_ValidPage_ReturnsValidResults()
    {
        // Arrange
        var data = Enumerable.Range(1, 50).AsQueryable();
        PaginationProperties properties = new(2, 10);

        // Act
        var page = data.ApplyPagination(properties);

        // Assert
        Assert.That(page, Is.EquivalentTo(Enumerable.Range(11, 10)));
    }

    [Test]
    public void ApplyPagination_WrongPage_ReturnsEmptyResults()
    {
        // Arrange
        var data = Enumerable.Range(1, 50).AsQueryable();
        PaginationProperties properties = new(111, 10);

        // Act
        var page = data.ApplyPagination(properties);

        // Assert
        Assert.That(page, Is.Empty);
    }
}
