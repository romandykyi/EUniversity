using EUniversity.Core.Dtos.University.Grades;
using EUniversity.Core.Filters;
using EUniversity.Core.Models;
using EUniversity.Core.Models.University;
using EUniversity.Core.Models.University.Grades;
using EUniversity.Core.Pagination;
using EUniversity.Core.Policy;
using EUniversity.Core.Services.University.Grades;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;

namespace EUniversity.IntegrationTests.Services;

public class AssignedGradesServiceTests : ServicesTest
{
    private IAssignedGradesService _service;
    private Course _testCourse = null!;
    private Group _testGroup = null!;
    private Grade _testGrade = null!;
    private ActivityType _testActivityType = null!;
    private ApplicationUser _testTeacher = null!;
    private ApplicationUser _testStudent1 = null!;
    private ApplicationUser _testStudent2 = null!;


    [SetUp]
    public async Task SetUp()
    {
        _service = ServiceScope.ServiceProvider.GetService<IAssignedGradesService>()!;

        _testCourse = new()
        {
            Name = "TestCourse"
        };
        DbContext.Add(_testCourse);
        await DbContext.SaveChangesAsync();
        _testTeacher = await RegisterTestUserAsync(Roles.Teacher);
        _testGroup = new()
        {
            Name = "TestGroup",
            TeacherId = _testTeacher.Id,
            CourseId = _testCourse.Id
        };
        DbContext.Add(_testGroup);
        await DbContext.SaveChangesAsync();

        _testStudent1 = await RegisterTestUserAsync(Roles.Student);
        _testStudent2 = await RegisterTestUserAsync(Roles.Student);
        StudentGroup studentGroup1 = new()
        {
            StudentId = _testStudent1.Id,
            GroupId = _testGroup.Id
        };
        StudentGroup studentGroup2 = new()
        {
            StudentId = _testStudent2.Id,
            GroupId = _testGroup.Id
        };
        DbContext.Add(studentGroup1);
        DbContext.Add(studentGroup2);
        await DbContext.SaveChangesAsync();

        _testActivityType = new()
        {
            Name = "TestActivity"
        };
        _testGrade = new()
        {
            Name = "TestGrade"
        };
        DbContext.Add(_testActivityType);
        DbContext.Add(_testGrade);
        await DbContext.SaveChangesAsync();
    }

    private AssignedGrade GetTestAssignedGrade()
    {
        return new()
        {
            GradeId = _testGrade.Id,
            StudentId = _testStudent1.Id,
            ActivityTypeId = _testActivityType.Id,
            GroupId = _testGroup.Id,
            AssignerId = _testTeacher.Id,
        };
    }

    private async Task<AssignedGrade> CreateTestAssignedGradeAsync()
    {
        AssignedGrade assignedGrade = GetTestAssignedGrade();
        DbContext.Add(assignedGrade);
        await DbContext.SaveChangesAsync();

        return assignedGrade;
    }

    [Test]
    public async Task GetPage_AppliesFilter()
    {
        // Arrange
        var filter = Substitute.For<IFilter<AssignedGrade>>();
        filter
            .Apply(Arg.Any<IQueryable<AssignedGrade>>())
            .Returns(x => x[0]);
        PaginationProperties properties = new(1, 20);

        // Act
        await _service.GetPageAsync<AssignedGradeViewDto>(properties, filter);

        // Assert
        filter.Received(1)
            .Apply(Arg.Any<IQueryable<AssignedGrade>>());
    }

    [Test]
    public async Task GetPage_ReceivesPaginationProperties()
    {
        // Arrange
        PaginationProperties properties = new(3, PaginationProperties.MinPageSize);

        // Act
        var result = await _service.GetPageAsync<AssignedGradeViewDto>(properties);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result.PageNumber, Is.EqualTo(properties.Page));
            Assert.That(result.PageSize, Is.EqualTo(properties.PageSize));
        });
    }

    [Test]
    public async Task GetPage_ReturnsCorrectTotalItemsCount()
    {
        // Arrange
        var testEntity = await CreateTestAssignedGradeAsync();
        int expectedCount = 1;

        var filter = Substitute.For<IFilter<AssignedGrade>>();
        filter.Apply(Arg.Any<IQueryable<AssignedGrade>>())
            .Returns(x =>
            {
                var query = (IQueryable<AssignedGrade>)x[0];
                return query.Where(e => e.Id.Equals(testEntity.Id));
            });
        PaginationProperties properties = new(1, 20);

        // Act
        var result = await _service.GetPageAsync<AssignedGradeViewDto>(properties, filter);

        // Assert
        Assert.That(result.TotalItemsCount, Is.EqualTo(expectedCount));
    }

    [Test]
    public async Task AssignAsync_ValidInput_CreatesAssignedGrade()
    {
        // Arrange
        AssignedGradeCreateDto dto = new(_testGrade.Id, _testGroup.Id, _testStudent1.Id, null, _testActivityType.Id);

        // Act
        AssignedGrade assignedGrade = await _service.AssignAsync(dto, _testTeacher.Id);

        // Assert that entity is actually created)
        AssignedGrade? actualGrade = await DbContext.AssignedGrades
            .AsNoTracking()
            .Where(g => g.StudentId == _testStudent1.Id && g.AssignerId == _testTeacher.Id)
            .FirstOrDefaultAsync();
        Assert.That(actualGrade, Is.Not.Null, "Grade was not assigned properly");
        Assert.Multiple(() =>
        {
            // Assert that creation date was set
            Assert.That(DateTimeOffset.Now - actualGrade.CreationDate,
                Is.LessThan(TimeSpan.FromHours(1)));
            // Assert that update date was set
            Assert.That(DateTimeOffset.Now - actualGrade.UpdateDate,
                Is.LessThan(TimeSpan.FromHours(1)));
        });
    }

    [Test]
    public async Task ReassignAsync_ElementExists_Succeeds()
    {
        // Arrange
        var assignedGrade = await CreateTestAssignedGradeAsync();
        AssignedGradeUpdateDto dto = new(_testGrade.Id, "New notes!", null);

        // Act
        bool result = await _service.ReassignAsync(assignedGrade.Id, dto, _testTeacher.Id);

        // Assert
        Assert.That(result);
        var actualEntity = await DbContext.Set<AssignedGrade>()
            .FirstOrDefaultAsync(c => c.Id.Equals(assignedGrade.Id));
        // Check if element was updated
        Assert.That(actualEntity, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(actualEntity.ReassignerId, Is.EqualTo(_testTeacher.Id));
            Assert.That(actualEntity.Notes, Is.EqualTo(dto.Notes));
            Assert.That(actualEntity.ActivityTypeId, Is.EqualTo(null));
        });
        // Assert that update date was set
        Assert.That(DateTimeOffset.Now - assignedGrade.UpdateDate,
            Is.LessThan(TimeSpan.FromHours(1)));
    }

    [Test]
    public async Task ReassignAsync_ElementDoesNotExist_ReturnsFalse()
    {
        // Arrange
        AssignedGradeUpdateDto dto = new(_testGrade.Id, "New notes!", null);

        // Act
        bool result = await _service.ReassignAsync(-1, dto, _testTeacher.Id);

        // Arrange
        Assert.That(result, Is.False);
    }

    [Test]
    public async Task Delete_ElementExists_Succeeds()
    {
        // Arrange
        var assignedGrade = await CreateTestAssignedGradeAsync();

        // Act
        bool result = await _service.DeleteAsync(assignedGrade.Id);

        // Assert
        var actualAssignedGrade = await DbContext.AssignedGrades
            .AsNoTracking()
            .Where(g => g.Id == assignedGrade.Id)
            .FirstOrDefaultAsync();
        Assert.Multiple(() =>
        {
            Assert.That(result);
            Assert.That(actualAssignedGrade, Is.Null, "Assigned grade wasn't deleted");
        });
    }

    [Test]
    public async Task Delete_ElementDoesNotExist_ReturnsFalse()
    {
        // Act
        bool result = await _service.DeleteAsync(-1);

        // Assert
        Assert.That(result, Is.False);
    }

    [Test]
    public async Task GetAssignerId_GradeExists_ReturnsGroupOwner()
    {
        // Arrange
        var grade = await CreateTestAssignedGradeAsync();

        // Act
        var result = await _service.GetAssignerIdAsync(grade.Id);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result.GradeExists, Is.True);
            Assert.That(result.AssignerId, Is.EqualTo(grade.AssignerId));
        });
    }

    [Test]
    public async Task GetAssignerId_GradeDoesNotExist_ReturnsCorrectResponse()
    {
        // Act
        var result = await _service.GetAssignerIdAsync(-1);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result.GradeExists, Is.False);
            Assert.That(result.AssignerId, Is.Null);
        });
    }
}
