using EUniversity.Core.Dtos.University;
using EUniversity.Core.Models.University;

namespace EUniversity.IntegrationTests.Controllers.University
{
    public class CoursesControllerTests :
        AdminCrudControllersTest<Course, int, PreviewCourseDto, ViewCourseDto, CreateCourseDto, CreateCourseDto>
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

        protected override CreateCourseDto GetInvalidCreateDto()
        {
            return new(string.Empty, null);
        }

        protected override CreateCourseDto GetInvalidUpdateDto()
        {
            return new(string.Empty, null);
        }

        protected override ViewCourseDto GetTestDetailsDto()
        {
            return new(DefaultId, "Test", null);
        }

        protected override PreviewCourseDto GetTestPreviewDto()
        {
            return new(DefaultId, "Test");
        }

        protected override CreateCourseDto GetValidCreateDto()
        {
            return new("Test", "test");
        }

        protected override CreateCourseDto GetValidUpdateDto()
        {
            return new("Test", "test2");
        }
    }
}
