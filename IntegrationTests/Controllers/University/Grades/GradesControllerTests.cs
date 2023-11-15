using EUniversity.Core.Dtos.University.Grades;
using EUniversity.Core.Filters;
using EUniversity.Core.Models.University.Grades;

namespace EUniversity.IntegrationTests.Controllers.University.Grades;

public class GradesControllerTestsAdminCrudControllersTest :
    AdminCrudControllersTest<Grade, int, GradeViewDto, GradeViewDto, GradeCreateDto, GradeCreateDto>
{
    public override string GetPageRoute => $"api/grades";

    public override string GetByIdRoute => $"api/grades/{DefaultId}";

    public override string PostRoute => $"api/grades/";

    public override string PutRoute => $"api/grades/{DefaultId}";

    public override string DeleteRoute => $"api/grades/{DefaultId}";

    public override int DefaultId => 1;

    public override string GetPageFilter => "name=testfilter";

    public override void SetUpService()
    {
        ServiceMock = WebApplicationFactory.GradesServiceMock;
    }

    protected override GradeViewDto GetTestPreviewDto()
    {
        return new(1, "100", 100);
    }

    protected override GradeViewDto GetTestDetailsDto()
    {
        return new(2, "100", 100);
    }

    protected override GradeCreateDto GetInvalidCreateDto()
    {
        return new(string.Empty, 0);
    }

    protected override GradeCreateDto GetInvalidUpdateDto()
    {
        return new(string.Empty, 0);
    }

    protected override GradeCreateDto GetValidCreateDto()
    {
        return new("100", 100);
    }

    protected override GradeCreateDto GetValidUpdateDto()
    {
        return new("100", 100);
    }

    protected override bool AssertThatFilterWasApplied(IFilter<Grade> filter)
    {
        return filter is NameFilter<Grade> nameFilter && nameFilter.Name == "testfilter";

    }
}
