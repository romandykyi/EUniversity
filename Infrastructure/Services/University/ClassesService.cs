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

    /// <summary>
    /// Creates a new class or classes based on the information provided in the
    /// <see cref="ClassCreateDto" /> asynchronously.
    /// </summary>
    /// <param name="dto">The DTO containing data for creating a class/classes.</param>
    /// <returns>
    /// A task that represents the asynchronous operation, containing
    /// one of the newly created classes.
    /// </returns>
    public async override Task<Class> CreateAsync(ClassCreateDto dto)
    {
        // Create a base class
        Class @class = dto.Adapt<Class>();
        DateTimeOffset currentDate = DateTimeOffset.Now;
        @class.CreationDate = @class.UpdateDate = currentDate;
        DbContext.Add(@class);

        // Create other classes with start date offset
        if (dto.Repeats != null && dto.RepeatsDelayDays != null)
        {
            for (int i = 1; i < dto.Repeats.Value; i++)
            {
                Class classDuplicate = dto.Adapt<Class>();
                classDuplicate.StartDate = dto.StartDate.AddDays(dto.RepeatsDelayDays.Value * i);
                classDuplicate.CreationDate = classDuplicate.UpdateDate = currentDate;
                DbContext.Add(classDuplicate);
            }
        }
        await DbContext.SaveChangesAsync();

        return @class;
    }
}
