using EUniversity.Core.Dtos.University;
using EUniversity.Core.Models.University;

namespace EUniversity.IntegrationTests.Controllers.University
{
    public class ClassroomsControllerTests :
        AdminCrudControllersTest<Classroom, int, ViewClassroomDto, ViewClassroomDto, CreateClassromDto, CreateClassromDto>
    {
        public override string GetPageRoute => $"api/classrooms";

        public override string GetByIdRoute => $"api/classrooms/{DefaultId}";

        public override string PostRoute => $"api/classrooms/";

        public override string PutRoute => $"api/classrooms/{DefaultId}";

        public override string DeleteRoute => $"api/classrooms/{DefaultId}";

        public override int DefaultId => 1;

        public override void SetUpService()
        {
            ServiceMock = WebApplicationFactory.ClassroomsServiceMock;
        }

        protected override ViewClassroomDto GetTestPreviewDto()
        {
            return new("Test");
        }

        protected override ViewClassroomDto GetTestDetailsDto()
        {
            return new("Test classroom");
        }

        protected override CreateClassromDto GetInvalidCreateDto()
        {
            return new(string.Empty);
        }

        protected override CreateClassromDto GetInvalidUpdateDto()
        {
            return new(string.Empty);
        }

        protected override CreateClassromDto GetValidCreateDto()
        {
            return new("#100");
        }

        protected override CreateClassromDto GetValidUpdateDto()
        {
            return new("#200");
        }
    }
}
