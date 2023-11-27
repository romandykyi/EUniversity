using EUniversity.Core.Dtos.University;
using EUniversity.Core.Models.University;
using EUniversity.Core.Services.University;

namespace EUniversity.IntegrationTests.Services.University;

public class CoursesServiceTests :
    CrudServicesTest<ICoursesService, Course, int, CoursePreviewDto, CourseViewDto, CourseCreateDto, CourseCreateDto>
{
    private Semester _testSemester;

    public static Course CreateTestCourse(Semester? testSemester = null)
    {
        return new()
        {
            Name = "Chemistry",
            Description = "Walter's White cooking course",
            Semester = testSemester,
            SemesterId = testSemester?.Id
        };
    }

    /// <inheritdoc />
    protected override void AssertThatWasUpdated(Course actualEntity, CourseCreateDto updateDto)
    {
        Assert.Multiple(() =>
        {
            Assert.That(actualEntity.Name, Is.EqualTo(updateDto.Name));
            Assert.That(actualEntity.Description, Is.EqualTo(updateDto.Description));
            Assert.That(actualEntity.SemesterId, Is.EqualTo(updateDto.SemesterId));
        });
    }

    /// <inheritdoc />
    protected override int GetNonExistentId()
    {
        return -1;
    }

    /// <inheritdoc />
    protected override Course GetTestEntity()
    {
        return CreateTestCourse();
    }

    /// <inheritdoc />
    protected override CourseCreateDto GetValidCreateDto()
    {
        return new("Physics", "g/(pi^2) = 1", _testSemester.Id);
    }

    /// <inheritdoc />
    protected override CourseCreateDto GetValidUpdateDto()
    {
        return new("Math", "Not to be confused with meth", null);
    }

    [SetUp]
    public async Task SetUpDependencies()
    {
        _testSemester = SemestersServiceTests.GetTestSemester();
        DbContext.Add(_testSemester);
        await DbContext.SaveChangesAsync();
    }
}
