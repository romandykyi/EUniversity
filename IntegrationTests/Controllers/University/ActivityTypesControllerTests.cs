using EUniversity.Core.Dtos.University;
using EUniversity.Core.Filters;
using EUniversity.Core.Models.University;
using EUniversity.Infrastructure.Filters;

namespace EUniversity.IntegrationTests.Controllers.University;

public class ActivityTypesControllerTests :
    AdminCrudControllersTest<ActivityType, int, ActivityTypeViewDto, ActivityTypeViewDto, ActivityTypeCreateDto, ActivityTypeCreateDto>
{
    public override string GetPageRoute => $"api/activityTypes";

    public override string GetByIdRoute => $"api/activityTypes/{DefaultId}";

    public override string PostRoute => $"api/activityTypes/";

    public override string PutRoute => $"api/activityTypes/{DefaultId}";

    public override string DeleteRoute => $"api/activityTypes/{DefaultId}";

    public override int DefaultId => 1;

    public override string GetPageFilter => "name=testfilter&sortingMode=name";

    public override void SetUpService()
    {
        ServiceMock = WebApplicationFactory.ActivityTypesServiceMock;
    }

    protected override ActivityTypeViewDto GetTestPreviewDto()
    {
        return new(1, "Presentation", DateTimeOffset.Now, DateTimeOffset.Now);
    }

    protected override ActivityTypeViewDto GetTestDetailsDto()
    {
        return new(2, "Exam", DateTimeOffset.Now, DateTimeOffset.Now);
    }

    protected override ActivityTypeCreateDto GetInvalidCreateDto()
    {
        return new(string.Empty);
    }

    protected override ActivityTypeCreateDto GetInvalidUpdateDto()
    {
        return new(string.Empty);
    }

    protected override ActivityTypeCreateDto GetValidCreateDto()
    {
        return new("Test");
    }

    protected override ActivityTypeCreateDto GetValidUpdateDto()
    {
        return new("Project");
    }

    protected override bool AssertThatFilterWasApplied(IFilter<ActivityType> filter)
    {
        return filter is DefaultFilter<ActivityType> defaultFilter &&
            defaultFilter.Name == "testfilter" &&
            defaultFilter.SortingMode == DefaultFilterSortingMode.Name;
    }
}
