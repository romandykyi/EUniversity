using EUniversity.Core.Dtos.University;
using EUniversity.Core.Dtos.Users;
using EUniversity.Core.Filters;
using EUniversity.Core.Models.University;
using EUniversity.Infrastructure.Filters;

namespace EUniversity.IntegrationTests.Controllers.University;

public class SemestersControllerTests :
    CrudControllersTest<Semester, int, SemesterPreviewDto, SemesterViewDto, SemesterCreateDto, SemesterCreateDto>
{
    public override string GetPageRoute => "api/semesters";

    public override string GetPageFilter => "name=testfilter&sortingMode=name";

    public override string GetByIdRoute => $"api/semesters/{DefaultId}";

    public override string PostRoute => "api/semesters";

    public override string PutRoute => $"api/semesters/{DefaultId}";

    public override string DeleteRoute => $"api/semesters/{DefaultId}";

    public override int DefaultId => 1;

    public override void SetUpService()
    {
        ServiceMock = WebApplicationFactory.SemestersServiceMock;
    }

    protected override bool AssertThatFilterWasApplied(IFilter<Semester> filter)
    {
        return filter is SemestersFilter semestersFilter &&
            semestersFilter.Name == "testfilter" &&
            semestersFilter.SortingMode == DefaultFilterSortingMode.Name;
    }

    protected override SemesterCreateDto GetInvalidCreateDto()
    {
        return new(string.Empty, DateTimeOffset.Now, DateTimeOffset.Now);
    }

    protected override SemesterCreateDto GetInvalidUpdateDto()
    {
        return GetInvalidCreateDto();
    }

    protected override SemesterViewDto GetTestDetailsDto()
    {
        return new(DefaultId, "Test semester",
           DateTimeOffset.Now, DateTimeOffset.Now,
            DateTimeOffset.MinValue, DateTimeOffset.MaxValue);
    }

    protected override SemesterPreviewDto GetTestPreviewDto()
    {
        return new(DefaultId, "Test semester",
            DateTimeOffset.Now, DateTimeOffset.Now,
            DateTimeOffset.MinValue, DateTimeOffset.MaxValue);
    }

    protected override SemesterCreateDto GetValidCreateDto()
    {
        return new("New semester", DateTimeOffset.MinValue, DateTimeOffset.MaxValue);
    }

    protected override SemesterCreateDto GetValidUpdateDto()
    {
        return new("Updated semester",
            DateTimeOffset.MinValue + TimeSpan.FromDays(1),
            DateTimeOffset.MaxValue - TimeSpan.FromDays(1));
    }
}
