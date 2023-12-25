using EUniversity.Core.Models;
using EUniversity.Core.Models.University;
using EUniversity.Core.Models.University.Grades;
using EUniversity.Infrastructure.Filters;
using System.ComponentModel;

namespace EUniversity.Tests.Filters;

public class AssignedGradesFilterTests
{
    private static readonly Grade TestGrade1 = new()
    {
        Id = 100,
        Name = "5"
    };
    private static readonly Grade TestGrade2 = new()
    {
        Id = 101,
        Name = "2"
    };
    private static readonly ApplicationUser TestTeacher1 = new()
    {
        Id = "test-teacher-1"
    };
    private static readonly ApplicationUser TestTeacher2 = new()
    {
        Id = "test-teacher-2"
    };
    private static readonly ApplicationUser TestStudent1 = new()
    {
        Id = "test-student-1"
    };
    private static readonly ApplicationUser TestStudent2 = new()
    {
        Id = "test-student-2"
    };
    private static readonly Group TestGroup1 = new()
    {
        Id = 200,
        Name = "Group1"
    };
    private static readonly Group TestGroup2 = new()
    {
        Id = 201,
        Name = "Group2"
    };
    private static readonly ActivityType TestActivityType = new()
    {
        Id = 200,
        Name = "Exam"
    };

    private static readonly AssignedGrade[] TestData =
    {
        new() 
        {
            Id = 500, 
            Grade = TestGrade1, GradeId = TestGrade1.Id, 
            AssignerId = TestTeacher1.Id, ReassignerId = TestTeacher2.Id, 
            StudentId = TestStudent1.Id, 
            GroupId = TestGroup2.Id,
            ActivityTypeId = TestActivityType.Id
        },
        new() 
        {
            Id = 501, 
            Grade = TestGrade1, GradeId = TestGrade1.Id, 
            AssignerId = TestTeacher2.Id, 
            StudentId = TestStudent1.Id, 
            GroupId = TestGroup1.Id,
            ActivityTypeId = TestActivityType.Id
        },
        new() 
        {
            Id = 502, 
            Grade = TestGrade2, GradeId = TestGrade2.Id, 
            AssignerId = TestTeacher1.Id, ReassignerId = TestTeacher2.Id, 
            StudentId = TestStudent1.Id, 
            GroupId = TestGroup1.Id
        },
        new() 
        {
            Id = 503, 
            Grade = TestGrade2, GradeId = TestGrade2.Id, 
            AssignerId = TestTeacher2.Id, 
            StudentId = TestStudent1.Id, 
            GroupId = TestGroup2.Id,
            ActivityTypeId = TestActivityType.Id
        },
        new() 
        {
            Id = 504, 
            Grade = TestGrade2, GradeId = TestGrade2.Id, 
            AssignerId = TestTeacher1.Id, ReassignerId = TestTeacher2.Id, 
            StudentId = TestStudent2.Id, 
            GroupId = TestGroup1.Id,
            ActivityTypeId = TestActivityType.Id
        },
        new() 
        {
            Id = 505, 
            Grade = TestGrade2, GradeId = TestGrade2.Id, 
            AssignerId = TestTeacher2.Id, ReassignerId = TestTeacher1.Id,
            StudentId = TestStudent1.Id, 
            GroupId = TestGroup1.Id
        },
    };

    [Test]
    public void AssignerIdSpecified_ReturnsFilteredQuery()
    {
        // Arrange
        AssignedGradesFilterProperties properties = new(AssignerId: TestTeacher1.Id);
        AssignedGradesFilter filter = new(properties);
        int[] expectedIds = { 500, 502, 504 };

        // Act
        var actualIds = filter.Apply(TestData.AsQueryable())
            .Select(x => x.Id);

        // Assert
        Assert.That(actualIds, Is.EquivalentTo(expectedIds));
    }

    [Test]
    public void ReassignerIdSpecified_ReturnsFilteredQuery()
    {
        // Arrange
        AssignedGradesFilterProperties properties = new(ReassignerId: TestTeacher1.Id);
        AssignedGradesFilter filter = new(properties);
        int[] expectedIds = { 505 };

        // Act
        var actualIds = filter.Apply(TestData.AsQueryable())
            .Select(x => x.Id);

        // Assert
        Assert.That(actualIds, Is.EquivalentTo(expectedIds));
    }

    [Test]
    public void StudentIdSpecified_ReturnsFilteredQuery()
    {
        // Arrange
        AssignedGradesFilterProperties properties = new(StudentId: TestStudent2.Id);
        AssignedGradesFilter filter = new(properties);
        int[] expectedIds = { 504 };

        // Act
        var actualIds = filter.Apply(TestData.AsQueryable())
            .Select(x => x.Id);

        // Assert
        Assert.That(actualIds, Is.EquivalentTo(expectedIds));
    }

    [Test]
    public void GroupIdSpecified_ReturnsFilteredQuery()
    {
        // Arrange
        AssignedGradesFilterProperties properties = new(GroupId: TestGroup2.Id);
        AssignedGradesFilter filter = new(properties);
        int[] expectedIds = { 500, 503 };

        // Act
        var actualIds = filter.Apply(TestData.AsQueryable())
            .Select(x => x.Id);

        // Assert
        Assert.That(actualIds, Is.EquivalentTo(expectedIds));
    }

    [Test]
    public void ActivityTypeId_Zero_ReturnsFilteredQuery()
    {
        // Arrange
        AssignedGradesFilterProperties properties = new(ActivityTypeId: 0);
        AssignedGradesFilter filter = new(properties);
        int[] expectedIds = { 502, 505 };

        // Act
        var actualIds = filter.Apply(TestData.AsQueryable())
            .Select(x => x.Id);

        // Assert
        Assert.That(actualIds, Is.EquivalentTo(expectedIds));
    }

    [Test]
    public void ActivityTypeIdSpecified_ReturnsFilteredQuery()
    {
        // Arrange
        AssignedGradesFilterProperties properties = new(ActivityTypeId: TestActivityType.Id);
        AssignedGradesFilter filter = new(properties);
        int[] expectedIds = { 500, 501, 503, 504 };

        // Act
        var actualIds = filter.Apply(TestData.AsQueryable())
            .Select(x => x.Id);

        // Assert
        Assert.That(actualIds, Is.EquivalentTo(expectedIds));
    }

    [Test]
    public void GradeIdSpecified_ReturnsFilteredQuery()
    {
        // Arrange
        AssignedGradesFilterProperties properties = new(GradeId: TestGrade1.Id);
        AssignedGradesFilter filter = new(properties);
        int[] expectedIds = { 500, 501 };

        // Act
        var actualIds = filter.Apply(TestData.AsQueryable())
            .Select(x => x.Id);

        // Assert
        Assert.That(actualIds, Is.EquivalentTo(expectedIds));
    }
}
