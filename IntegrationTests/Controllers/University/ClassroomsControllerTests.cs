using EUniversity.Core.Dtos.University;
using EUniversity.Core.Models.University;

namespace EUniversity.IntegrationTests.Controllers.University
{
    public class ClassroomsControllerTests :
        AdminCrudControllersTest<Classroom, int, ViewClassroomDto, ViewClassroomDto, CreateClassroomDto, CreateClassroomDto>
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
            return new(1, "Test");
        }

        protected override ViewClassroomDto GetTestDetailsDto()
        {
            return new(2, "Test classroom");
        }

        protected override CreateClassroomDto GetInvalidCreateDto()
        {
            return new(string.Empty);
        }

        protected override CreateClassroomDto GetInvalidUpdateDto()
        {
            return new(string.Empty);
        }

        protected override CreateClassroomDto GetValidCreateDto()
        {
            return new("#100");
        }

        protected override CreateClassroomDto GetValidUpdateDto()
        {
            return new("#200");
        }
    }
}
