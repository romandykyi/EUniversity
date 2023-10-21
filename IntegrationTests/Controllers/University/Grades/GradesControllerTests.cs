using EUniversity.Core.Dtos.University.Grades;
using EUniversity.Core.Models.University.Grades;

namespace EUniversity.IntegrationTests.Controllers.University.Grades
{
    public class GradesControllerTestsAdminCrudControllersTest :
        AdminCrudControllersTest<Grade, int, ViewGradeDto, ViewGradeDto, CreateGradeDto, CreateGradeDto>
    {
        public override string GetPageRoute => $"api/grades";

        public override string GetByIdRoute => $"api/grades/{DefaultId}";

        public override string PostRoute => $"api/grades/";

        public override string PutRoute => $"api/grades/{DefaultId}";

        public override string DeleteRoute => $"api/grades/{DefaultId}";

        public override int DefaultId => 1;

        public override void SetUpService()
        {
            ServiceMock = WebApplicationFactory.GradesServiceMock;
        }

        protected override ViewGradeDto GetTestPreviewDto()
        {
            return new(1, "100", 100);
        }

        protected override ViewGradeDto GetTestDetailsDto()
        {
            return new(2, "100", 100);
        }

        protected override CreateGradeDto GetInvalidCreateDto()
        {
            return new(string.Empty, 0);
        }

        protected override CreateGradeDto GetInvalidUpdateDto()
        {
            return new(string.Empty, 0);
        }

        protected override CreateGradeDto GetValidCreateDto()
        {
            return new("100", 100);
        }

        protected override CreateGradeDto GetValidUpdateDto()
        {
            return new("100", 100);
        }
    }
}
