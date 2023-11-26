using EUniversity.Core.Dtos.University;
using EUniversity.Core.Filters;
using EUniversity.Core.Models.University;
using EUniversity.Core.Services.University;

namespace EUniversity.IntegrationTests.Controllers.University;

public class CoursesControllerTests :
    AdminCrudControllersTest<Course, int, CoursePreviewDto, CourseViewDto, CourseCreateDto, CourseCreateDto>
{
    public override string GetPageRoute => "api/courses";

    public override string GetByIdRoute => $"api/courses/{DefaultId}";

    public override string PostRoute => "api/courses";

    public override string PutRoute => $"api/courses/{DefaultId}";

    public override string DeleteRoute => $"api/courses/{DefaultId}";

    public override int DefaultId => 1;

    public override string GetPageFilter => "name=testfilter";

    public override void SetUpService()
    {
        ServiceMock = WebApplicationFactory.CoursesServiceMock;
        Assert.That(ServiceMock, Is.InstanceOf<ICoursesService>());
        SetUpValidationMocks();
    }

    protected override bool AssertThatFilterWasApplied(IFilter<Course> filter)
    {
        return filter is NameFilter<Course> nameFilter && nameFilter.Name == "testfilter";
    }

    protected override CourseCreateDto GetInvalidCreateDto()
    {
        return new(string.Empty, null, null);
    }

    protected override CourseCreateDto GetInvalidUpdateDto()
    {
        return GetInvalidCreateDto();
    }

    protected override CourseViewDto GetTestDetailsDto()
    {
        SemesterPreviewDto semester = new(4, "Test semester",
            DateTimeOffset.MinValue, DateTimeOffset.MaxValue, 
            DateTimeOffset.Now, DateTimeOffset.Now);
        return new(DefaultId, "Test", null, DateTimeOffset.Now, DateTimeOffset.Now, semester);
    }

    protected override CoursePreviewDto GetTestPreviewDto()
    {
        SemesterMinimalViewDto semester = new(4, "Test semester");
        return new(DefaultId, "Test", DateTimeOffset.Now, DateTimeOffset.Now, semester);
    }

    protected override CourseCreateDto GetValidCreateDto()
    {
        return new("Test", "test", null);
    }

    protected override CourseCreateDto GetValidUpdateDto()
    {
        return new("Test", "test2", 5);
    }
}
