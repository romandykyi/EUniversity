using EUniversity.Core.Models;
using EUniversity.Core.Models.University;
using EUniversity.Infrastructure.Filters;

namespace EUniversity.Tests.Filters;

public class ClassesFilterTests
{
    private static readonly ApplicationUser TestTeacher = new()
    {
        Id = "1",
        FirstName = "Test",
        LastName = "Teacher"
    };
    private static readonly ApplicationUser TestSubstituteTeacher = new()
    {
        Id = "2",
        FirstName = "Some",
        LastName = "Guy"
    };
    private static readonly ClassType TestClassType1 = new()
    {
        Id = 1,
        Name = "Test1"
    };
    private static readonly ClassType TestClassType2 = new()
    {
        Id = 2,
        Name = "Some type"
    };
    private static readonly Classroom TestClassroom1 = new()
    {
        Id = 1,
        Name = "TestClassroom1"
    };
    private static readonly Classroom TestClassroom2 = new()
    {
        Id = 2,
        Name = "Some classroom"
    };
    private static readonly ApplicationUser TestStudent1 = new()
    {
        Id = "test-user-id1"
    };
    private static readonly ApplicationUser TestStudent2 = new()
    {
        Id = "test-user-id2"
    };
    private static readonly Group TestGroup1 = new()
    {
        Id = 1,
        Name = "TestGroup1",
        Teacher = TestTeacher,
        TeacherId = TestTeacher.Id,
        Students = new[] { TestStudent1 }
    };
    private static readonly Group TestGroup2 = new()
    {
        Id = 2,
        Name = "Some group",
        Teacher = TestTeacher,
        TeacherId = TestTeacher.Id,
        Students = new[] { TestStudent2 }
    };

    private static Class GetClass(int id, ClassType type, Classroom classroom,
        Group group, DateTimeOffset startDate, ApplicationUser? substituteTeacher = null)
    {
        Class @class = new()
        {
            Id = id,
            ClassType = type,
            ClassTypeId = type.Id,
            Classroom = classroom,
            ClassroomId = classroom.Id,
            Group = group,
            GroupId = group.Id,
            StartDate = startDate
        };
        if (substituteTeacher != null)
        {
            @class.SubstituteTeacher = substituteTeacher;
            @class.SubstituteTeacherId = substituteTeacher.Id;
        }
        return @class;
    }

    private Class[] TestClasses =
    {
        GetClass(1, TestClassType1, TestClassroom1, TestGroup1, new(3L, TimeSpan.Zero)),
        GetClass(2, TestClassType1, TestClassroom1, TestGroup1, new(1L, TimeSpan.Zero), TestSubstituteTeacher),
        GetClass(3, TestClassType1, TestClassroom1, TestGroup2, new(2L, TimeSpan.Zero)),
        GetClass(4, TestClassType1, TestClassroom2, TestGroup1, new(4L, TimeSpan.Zero)),
        GetClass(5, TestClassType2, TestClassroom1, TestGroup1, new(6L, TimeSpan.Zero)),
        GetClass(6, TestClassType1, TestClassroom1, TestGroup2, new(5L, TimeSpan.Zero), TestSubstituteTeacher)
    };

    [Test]
    public void EmptyFilter_ReturnsSortedByStartDateQuery()
    {
        // Arrange
        ClassesFilterProperties properties = new();
        ClassesFilter filter = new(properties);

        // Act
        var result = filter.Apply(TestClasses.AsQueryable());

        // Assert
        Assert.That(result, Is.EquivalentTo(TestClasses));
        Assert.That(result, Is.Ordered.By("StartDate"));
    }

    [Test]
    public void StudentIdSpecified_ReturnsFilteredQuery()
    {
        // Arrange
        ClassesFilterProperties properties = new(StudentId: TestStudent1.Id);
        ClassesFilter filter = new(properties);
        int[] expectedIds = { 1, 2, 4, 5 };

        // Act
        var actualIds = filter
            .Apply(TestClasses.AsQueryable())
            .Select(c => c.Id);

        // Assert
        Assert.That(actualIds, Is.EquivalentTo(expectedIds));
    }

    [Test]
    public void TeacherIdSpecified_ReturnsFilteredQuery()
    {
        // Arrange
        ClassesFilterProperties properties = new(TeacherId: TestTeacher.Id);
        ClassesFilter filter = new(properties);
        int[] expectedIds = { 1, 3, 4, 5 };

        // Act
        var actualIds = filter
            .Apply(TestClasses.AsQueryable())
            .Select(c => c.Id);

        // Assert
        Assert.That(actualIds, Is.EquivalentTo(expectedIds));
    }

    [Test]
    public void GroupIdSpecified_ReturnsFilteredQuery()
    {
        // Arrange
        ClassesFilterProperties properties = new(GroupId: TestGroup1.Id);
        ClassesFilter filter = new(properties);
        int[] expectedIds = { 1, 2, 4, 5 };

        // Act
        var actualIds = filter
            .Apply(TestClasses.AsQueryable())
            .Select(c => c.Id);

        // Assert
        Assert.That(actualIds, Is.EquivalentTo(expectedIds));
    }

    [Test]
    public void ClassroomIdSpecified_ReturnsFilteredQuery()
    {
        // Arrange
        ClassesFilterProperties properties = new(ClassroomId: TestClassroom1.Id);
        ClassesFilter filter = new(properties);
        int[] expectedIds = { 1, 2, 3, 5, 6 };

        // Act
        var actualIds = filter
            .Apply(TestClasses.AsQueryable())
            .Select(c => c.Id);

        // Assert
        Assert.That(actualIds, Is.EquivalentTo(expectedIds));
    }

    [Test]
    public void ClassroomTypeIdSpecified_ReturnsFilteredQuery()
    {
        // Arrange
        ClassesFilterProperties properties = new(ClassTypeId: TestClassType1.Id);
        ClassesFilter filter = new(properties);
        int[] expectedIds = { 1, 2, 3, 4, 6 };

        // Act
        var actualIds = filter
            .Apply(TestClasses.AsQueryable())
            .Select(c => c.Id);

        // Assert
        Assert.That(actualIds, Is.EquivalentTo(expectedIds));
    }

    [Test]
    public void MinStartDateSpecified_ReturnsFilteredQuery()
    {
        // Arrange
        ClassesFilterProperties properties = new(MinStartDate: new(4L, TimeSpan.Zero));
        ClassesFilter filter = new(properties);
        int[] expectedIds = { 4, 5, 6 };

        // Act
        var actualIds = filter
            .Apply(TestClasses.AsQueryable())
            .Select(c => c.Id);

        // Assert
        Assert.That(actualIds, Is.EquivalentTo(expectedIds));
    }

    [Test]
    public void MaxStartDateSpecified_ReturnsFilteredQuery()
    {
        // Arrange
        ClassesFilterProperties properties = new(MaxStartDate: new(4L, TimeSpan.Zero));
        ClassesFilter filter = new(properties);
        int[] expectedIds = { 1, 2, 3, 4 };

        // Act
        var actualIds = filter
            .Apply(TestClasses.AsQueryable())
            .Select(c => c.Id);

        // Assert
        Assert.That(actualIds, Is.EquivalentTo(expectedIds));
    }
}
