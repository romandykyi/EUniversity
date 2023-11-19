using EUniversity.Core.Dtos.University;
using EUniversity.Core.Models.University;
using EUniversity.Core.Services.University;
using EUniversity.Infrastructure.Data;

namespace EUniversity.Infrastructure.Services.University;

public class SemestersService :
    BaseCrudService<Semester, int, SemesterPreviewDto, SemesterViewDto, SemesterCreateDto, SemesterCreateDto>,
    ISemestersService
{
    public SemestersService(ApplicationDbContext dbContext) : base(dbContext)
    {
    }
}
