using EUniversity.Core.Models;
using EUniversity.Infrastructure.Filters;

namespace EUniversity.Tests.Filters;

public class StudentEnrollmentsFilterTests
{
    public record TestEnrollment(DateTimeOffset EnrollmentDate, ApplicationUser? Student) :
        IStudentEnrollment;


    private readonly TestEnrollment[] TestEnrollments =
    {
        new(new DateTimeOffset(0, TimeSpan.Zero), new ApplicationUser() {FirstName = "Mark", LastName = "Black" }),
        new(new DateTimeOffset(2, TimeSpan.Zero), new ApplicationUser() {FirstName = "Marc", MiddleName = "John", LastName = "Johnson" }),
        new(new DateTimeOffset(1, TimeSpan.Zero), new ApplicationUser() {FirstName = "Joe", LastName = "Doe" }),
        new(new DateTimeOffset(3, TimeSpan.Zero), new ApplicationUser() {FirstName = "Jane", MiddleName = "Diana", LastName = "Doe" }),
    };

    [Test]
    public void EmptyProperties_ReturnsEntireQuery()
    {
        // Arrange
        StudentEnrollmentsFilterProperties properties = new();
        StudentEnrollmentsFilter<TestEnrollment> filter = new(properties);

        // Act
        var result = filter.Apply(TestEnrollments.AsQueryable());

        // Assert
        Assert.That(result, Is.EquivalentTo(TestEnrollments));
    }

    [Test]
    public void FullNameSpecified_ReturnsFilteredQuery()
    {
        // Arrange
        StudentEnrollmentsFilterProperties properties = new(FullName: "Mar");
        StudentEnrollmentsFilter<TestEnrollment> filter = new(properties);
        string[] expectedNames = { "Mark", "Marc" };

        // Act
        var result = filter.Apply(TestEnrollments.AsQueryable());

        // Assert
        Assert.That(result.Select(result => result.Student!.FirstName), Is.EquivalentTo(expectedNames));
    }

    [Test]
    public void FullNameSortingMode_ReturnsOrderedByFullNameQuery()
    {
        // Arrange
        StudentEnrollmentsFilterProperties properties = new(SortingMode: StudentEnrollmentsSortingMode.FullName);
        StudentEnrollmentsFilter<TestEnrollment> filter = new(properties);

        // Act
        var result = filter.Apply(TestEnrollments.AsQueryable());

        // Assert
        Assert.That(result, Is.EquivalentTo(TestEnrollments));
        Assert.That(result.Select(e => e.Student), Is.Ordered.Ascending.By("FirstName"));
    }

    [Test]
    public void FullNameDescendingSortingMode_ReturnsOrderedDescendingByFullNameQuery()
    {
        // Arrange
        StudentEnrollmentsFilterProperties properties = new(SortingMode: StudentEnrollmentsSortingMode.FullNameDescending);
        StudentEnrollmentsFilter<TestEnrollment> filter = new(properties);

        // Act
        var result = filter.Apply(TestEnrollments.AsQueryable());

        // Assert
        Assert.That(result, Is.EquivalentTo(TestEnrollments));
        Assert.That(result.Select(e => e.Student), Is.Ordered.Descending.By("FirstName"));
    }

    [Test]
    public void NewestSortingMode_ReturnsOrderedDescendingByEnrollmentDateQuery()
    {
        // Arrange
        StudentEnrollmentsFilterProperties properties = new(SortingMode: StudentEnrollmentsSortingMode.Newest);
        StudentEnrollmentsFilter<TestEnrollment> filter = new(properties);

        // Act
        var result = filter.Apply(TestEnrollments.AsQueryable());

        // Assert
        Assert.That(result, Is.EquivalentTo(TestEnrollments));
        Assert.That(result, Is.Ordered.Descending.By("EnrollmentDate"));
    }

    [Test]
    public void OldestSortingMode_ReturnsOrderedAscendingByEnrollmentDateQuery()
    {
        // Arrange
        StudentEnrollmentsFilterProperties properties = new(SortingMode: StudentEnrollmentsSortingMode.Oldest);
        StudentEnrollmentsFilter<TestEnrollment> filter = new(properties);

        // Act
        var result = filter.Apply(TestEnrollments.AsQueryable());

        // Assert
        Assert.That(result, Is.EquivalentTo(TestEnrollments));
        Assert.That(result, Is.Ordered.Ascending.By("EnrollmentDate"));
    }
}
