using EUniversity.Core.Dtos.University;
using EUniversity.Core.Filters;
using EUniversity.Core.Models.University;
using EUniversity.Infrastructure.Filters;

namespace EUniversity.IntegrationTests.Controllers.University;

public class ClassTypesControllerTests :
    AdminCrudControllersTest<ClassType, int, ClassTypeViewDto, ClassTypeViewDto, ClassTypeCreateDto, ClassTypeCreateDto>
{
    public override string GetPageRoute => $"api/classTypes";

    public override string GetByIdRoute => $"api/classTypes/{DefaultId}";

    public override string PostRoute => $"api/classTypes/";

    public override string PutRoute => $"api/classTypes/{DefaultId}";

    public override string DeleteRoute => $"api/classTypes/{DefaultId}";

    public override int DefaultId => 1;

    public override string GetPageFilter => "name=testfilter&sortingMode=name";

    public override void SetUpService()
    {
        ServiceMock = WebApplicationFactory.ClassTypesServiceMock;
    }

    protected override ClassTypeViewDto GetTestPreviewDto()
    {
        return new(1, "Test", DateTimeOffset.Now, DateTimeOffset.Now);
    }

    protected override ClassTypeViewDto GetTestDetailsDto()
    {
        return new(2, "Test", DateTimeOffset.Now, DateTimeOffset.Now);
    }

    protected override ClassTypeCreateDto GetInvalidCreateDto()
    {
        return new(string.Empty);
    }

    protected override ClassTypeCreateDto GetInvalidUpdateDto()
    {
        return new(string.Empty);
    }

    protected override ClassTypeCreateDto GetValidCreateDto()
    {
        return new("Lecture");
    }

    protected override ClassTypeCreateDto GetValidUpdateDto()
    {
        return new("Practical");
    }

    protected override bool AssertThatFilterWasApplied(IFilter<ClassType> filter)
    {
        return filter is DefaultFilter<ClassType> defaultFilter &&
            defaultFilter.Name == "testfilter" &&
            defaultFilter.SortingMode == DefaultFilterSortingMode.Name;
    }
}
