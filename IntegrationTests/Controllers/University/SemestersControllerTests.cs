using EUniversity.Core.Dtos.University;
using EUniversity.Core.Dtos.Users;
using EUniversity.Core.Filters;
using EUniversity.Core.Models.University;

namespace EUniversity.IntegrationTests.Controllers.University;

public class SemestersControllerTests :
    CrudControllersTest<Semester, int, SemesterPreviewDto, SemesterViewDto, SemesterCreateDto, SemesterCreateDto>
{
    public override string GetPageRoute => "api/semesters";

    public override string GetPageFilter => "name=testfilter";

    public override string GetByIdRoute => $"api/semesters/{DefaultId}";

    public override string PostRoute => "api/semesters";

    public override string PutRoute => $"api/semesters/{DefaultId}";

    public override string DeleteRoute => $"api/semesters/{DefaultId}";

    public override int DefaultId => 1;

    public override void SetUpService()
    {
        ServiceMock = WebApplicationFactory.SemestersServiceMock;
    }

    protected override void AssertThatViewDtosAreEqual(SemesterViewDto expected, SemesterViewDto actual)
    {
        Assert.Multiple(() =>
        {
            Assert.That(actual.Id, Is.EqualTo(expected.Id));
            Assert.That(actual.Name, Is.EqualTo(expected.Name));
            Assert.That(actual.DateFrom, Is.EqualTo(expected.DateFrom));
            Assert.That(actual.DateTo, Is.EqualTo(expected.DateTo));
            Assert.That(actual.StudentEnrollments, Is.EquivalentTo(expected.StudentEnrollments));
        });
    }

    protected override bool AssertThatFilterWasApplied(IFilter<Semester> filter)
    {
        return filter is NameFilter<Semester> nameFilter && nameFilter.Name == "testfilter";
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
        StudentPreviewDto student = new("test-id", "test-user", "Test", "User", null);
        List<StudentSemesterViewDto> enrollments = new()
        {
            new(student, DateTimeOffset.Now)
        };
        return new(DefaultId, "Test semester", DateTimeOffset.MinValue, DateTimeOffset.MaxValue, enrollments);
    }

    protected override SemesterPreviewDto GetTestPreviewDto()
    {
        return new(DefaultId, "Test semester", DateTimeOffset.MinValue, DateTimeOffset.MaxValue);
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
