using EUniversity.Core.Dtos.University;
using EUniversity.Core.Dtos.Users;
using EUniversity.Core.Filters;
using EUniversity.Core.Models.University;
using EUniversity.Core.Services.University;
using EUniversity.Infrastructure.Filters;

namespace EUniversity.IntegrationTests.Controllers.University;

public class ClassesControllerTests :
    AdminCrudControllersTest<Class, int, ClassViewDto, ClassViewDto, ClassCreateDto, ClassUpdateDto>
{
    public override string GetPageRoute => "api/classes";

    public override string GetByIdRoute => $"api/classes/{DefaultId}";

    public override string PostRoute => "api/classes";

    public override string PutRoute => $"api/classes/{DefaultId}";

    public override string DeleteRoute => $"api/classes/{DefaultId}";

    public override int DefaultId => 1;

    public override string GetPageFilter => "classroomId=5&classTypeId=2";

    public override void SetUpService()
    {
        ServiceMock = WebApplicationFactory.ClassesServiceMock;
        Assert.That(ServiceMock, Is.InstanceOf<IClassesService>());
        SetUpValidationMocks();
    }

    protected override ClassCreateDto GetInvalidCreateDto()
    {
        return new(1, 2, 3, null, DateTimeOffset.Now, TimeSpan.FromHours(1), -1, 7);
    }

    protected override ClassUpdateDto GetInvalidUpdateDto()
    {
        return new(1, 2, 3, null, DateTimeOffset.Now, TimeSpan.FromHours(-1));
    }

    protected override ClassViewDto GetTestDetailsDto()
    {
        TeacherPreviewDto teacher = new("test", "teacher", "Joe", "Doe", null);
        SemesterMinimalViewDto semester = new(3, "Semester");
        ClassCourseViewDto course = new(DefaultId, "Name", semester);
        ClassGroupViewDto group = new(DefaultId, "Group", teacher, course);
        return new ClassViewDto(
            DefaultId, DateTimeOffset.Now, TimeSpan.FromHours(1), 
            DateTimeOffset.Now, DateTimeOffset.Now, group, null);
    }

    protected override ClassViewDto GetTestPreviewDto()
    {
        return GetTestDetailsDto();
    }

    protected override ClassCreateDto GetValidCreateDto()
    {
        return new(1, 2, 3, null, DateTimeOffset.Now, TimeSpan.FromHours(1), 1, 7);
    }

    protected override ClassUpdateDto GetValidUpdateDto()
    {
        return new(1, 2, 3, null, DateTimeOffset.Now, TimeSpan.FromHours(1));
    }

    protected override bool AssertThatFilterWasApplied(IFilter<Class> filter)
    {
        return filter is ClassesFilter classesFilter &&
            classesFilter.Properties.ClassroomId == 5 &&
            classesFilter.Properties.ClassTypeId == 2;
    }
}
