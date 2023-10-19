using EUniversity.Core.Dtos.University.Grades;
using EUniversity.Core.Models.University.Grades;

namespace EUniversity.IntegrationTests.Controllers.University.Grades
{
    public class GradesControllerTestsAdminCrudControllersTest :
        AdminCrudControllersTest<Grade, int, GradeDto, GradeDto, GradeDto, GradeDto>
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

        protected override GradeDto GetTestPreviewDto()
        {
            return new("100", 100);
        }

        protected override GradeDto GetTestDetailsDto()
        {
            return new("100", 100);
        }

        protected override GradeDto GetInvalidCreateDto()
        {
            return new(string.Empty, 0);
        }

        protected override GradeDto GetInvalidUpdateDto()
        {
            return new(string.Empty, 0);
        }

        protected override GradeDto GetValidCreateDto()
        {
            return new("100", 100);
        }

        protected override GradeDto GetValidUpdateDto()
        {
            return new("100", 100);
        }
    }
}
