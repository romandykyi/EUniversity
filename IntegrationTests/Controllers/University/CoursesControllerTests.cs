using EUniversity.Core.Dtos.University;
using EUniversity.Core.Models.University;

namespace EUniversity.IntegrationTests.Controllers.University
{
    public class CoursesControllerTests :
        AdminCrudControllersTest<Course, int, CoursePreviewDto, CourseViewDto, CourseCreateDto, CourseCreateDto>
    {
        public override string GetPageRoute => "api/courses";

        public override string GetByIdRoute => $"api/courses/{DefaultId}";

        public override string PostRoute => "api/courses";

        public override string PutRoute => $"api/courses/{DefaultId}";

        public override string DeleteRoute => $"api/courses/{DefaultId}";

        public override int DefaultId => 1;

        public override void SetUpService()
        {
            ServiceMock = WebApplicationFactory.CoursesServiceMock;
        }

        protected override CourseCreateDto GetInvalidCreateDto()
        {
            return new(string.Empty, null);
        }

        protected override CourseCreateDto GetInvalidUpdateDto()
        {
            return new(string.Empty, null);
        }

        protected override CourseViewDto GetTestDetailsDto()
        {
            return new(DefaultId, "Test", null);
        }

        protected override CoursePreviewDto GetTestPreviewDto()
        {
            return new(DefaultId, "Test");
        }

        protected override CourseCreateDto GetValidCreateDto()
        {
            return new("Test", "test");
        }

        protected override CourseCreateDto GetValidUpdateDto()
        {
            return new("Test", "test2");
        }
    }
}
