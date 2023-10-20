using EUniversity.Core.Dtos.University;
using EUniversity.Core.Models.University;
using EUniversity.Core.Services.University;
using EUniversity.Infrastructure.Data;

namespace EUniversity.Infrastructure.Services.University
{
    public class CoursesService :
        BaseCrudService<Course, int, PreviewCourseDto, ViewCourseDto, CreateCourseDto, CreateCourseDto>,
        ICoursesService
    {
        public CoursesService(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
    }
}
