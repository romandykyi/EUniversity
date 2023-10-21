using EUniversity.Core.Dtos.University.Grades;
using EUniversity.Core.Models.University.Grades;
using EUniversity.Infrastructure.Data;

namespace EUniversity.Infrastructure.Services.University.Grades
{
    public class GradesService :
        BaseCrudService<Grade, int, ViewGradeDto, ViewGradeDto, CreateGradeDto, CreateGradeDto>,
        IGradesService
    {
        public GradesService(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
    }
}
