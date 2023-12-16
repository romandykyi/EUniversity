using EUniversity.Core.Models.University;
using EUniversity.Infrastructure.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EUniversity.Tests.Filters;

public class SemestersFilterTests
{
    private readonly Semester[] TestSemesters =
    {
        new Semester() {Id = 1, Name = "Semester I", DateFrom = new(1L, TimeSpan.Zero), DateTo = new(100L, TimeSpan.Zero)},
        new Semester() {Id = 2, Name = "Semester II", DateFrom = new(50L, TimeSpan.Zero), DateTo = new(150L, TimeSpan.Zero)},
        new Semester() {Id = 3, Name = "Semester III", DateFrom = new(101L, TimeSpan.Zero), DateTo = new(200L, TimeSpan.Zero)}
    };

    [Test]
    public void EmptyFilter_ReturnsQuery()
    {
        // Arrange
        SemestersFilterProperties properties = new();
        SemestersFilter filter = new(properties, string.Empty);

        // Act
        var result = filter
            .Apply(TestSemesters.AsQueryable());

        // Arrange
        Assert.That(result, Is.EquivalentTo(TestSemesters));
    }

    [Test]
    public void MaxDateFromSpecified_ReturnsFilteredQuery()
    {
        // Arrange
        DateTimeOffset date = new(75L, TimeSpan.Zero);
        SemestersFilterProperties properties = new(MaxDateFrom: date);
        SemestersFilter filter = new(properties, string.Empty);
        int[] expectedIds = { 1, 2 };

        // Act
        var actualIds = filter
            .Apply(TestSemesters.AsQueryable())
            .Select(s => s.Id);

        // Assert
        Assert.That(actualIds, Is.EquivalentTo(expectedIds));
    }

    [Test]
    public void MinDateToSpecified_ReturnsFilteredQuery()
    {
        // Arrange
        DateTimeOffset date = new(125L, TimeSpan.Zero);
        SemestersFilterProperties properties = new(MinDateTo: date);
        SemestersFilter filter = new(properties, string.Empty);
        int[] expectedIds = { 2, 3 };

        // Act
        var actualIds = filter
            .Apply(TestSemesters.AsQueryable())
            .Select(s => s.Id);

        // Assert
        Assert.That(actualIds, Is.EquivalentTo(expectedIds));
    }
}
