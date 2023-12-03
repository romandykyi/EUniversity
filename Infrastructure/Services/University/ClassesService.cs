using EUniversity.Core.Dtos.University;
using EUniversity.Core.Models.University;
using EUniversity.Core.Services.University;
using EUniversity.Infrastructure.Data;

namespace EUniversity.Infrastructure.Services.University;

/// <inheritdoc />
public class ClassesService : BaseCrudService<Class, int, ClassViewDto, ClassViewDto, ClassCreateDto, ClassUpdateDto>,
    IClassesService
{
    protected override IQueryable<Class> GetByIdQuery =>
        Entities
        .Include(c => c.Classroom)
        .Include(c => c.Group)
        .ThenInclude(g => g!.Course)
        .ThenInclude(c => c.Semester)
        .Include(c => c.Group)
        .ThenInclude(g => g!.Teacher)
        .Include(c => c.SubstituteTeacher)
        .AsNoTracking();

    protected override IQueryable<Class> GetPageQuery =>
        Entities
        .Include(c => c.Classroom)
        .Include(c => c.Group)
        .ThenInclude(g => g!.Course)
        .ThenInclude(c => c.Semester)
        .Include(c => c.Group)
        .ThenInclude(g => g!.Teacher)
        .Include(c => c.SubstituteTeacher)
        .AsNoTracking();

    public ClassesService(ApplicationDbContext dbContext) : base(dbContext)
    {
    }
}
