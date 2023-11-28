using EUniversity.Core.Dtos.University;
using EUniversity.Core.Models.University;
using EUniversity.Core.Services.University;
using EUniversity.Infrastructure.Data;

namespace EUniversity.Infrastructure.Services.University;

public class CoursesService :
    BaseCrudService<Course, int, CoursePreviewDto, CourseViewDto, CourseCreateDto, CourseCreateDto>,
    ICoursesService
{
    protected override IQueryable<Course> GetByIdQuery =>
        Entities
        .Include(e => e.Semester)
        .AsNoTracking();

    protected override IQueryable<Course> GetPageQuery =>
        Entities
        .Include(e => e.Semester)
        .AsNoTracking();

    public CoursesService(ApplicationDbContext dbContext) : base(dbContext)
    {
    }
}
