﻿using EUniversity.Core.Dtos.University;
using EUniversity.Core.Models.University;

namespace EUniversity.IntegrationTests.Controllers.University
{
    public class ClassroomsControllerTests :
        AdminCrudControllersTest<Classroom, int, ClassroomViewDto, ClassroomViewDto, ClassroomCreateDto, ClassroomCreateDto>
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

        protected override ClassroomViewDto GetTestPreviewDto()
        {
            return new(1, "Test");
        }

        protected override ClassroomViewDto GetTestDetailsDto()
        {
            return new(2, "Test classroom");
        }

        protected override ClassroomCreateDto GetInvalidCreateDto()
        {
            return new(string.Empty);
        }

        protected override ClassroomCreateDto GetInvalidUpdateDto()
        {
            return new(string.Empty);
        }

        protected override ClassroomCreateDto GetValidCreateDto()
        {
            return new("#100");
        }

        protected override ClassroomCreateDto GetValidUpdateDto()
        {
            return new("#200");
        }
    }
}
