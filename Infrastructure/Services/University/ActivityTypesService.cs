using EUniversity.Core.Dtos.University;
using EUniversity.Core.Models.University;
using EUniversity.Core.Services.University;
using EUniversity.Infrastructure.Data;

namespace EUniversity.Infrastructure.Services.University;

public class ActivityTypesService :
    BaseCrudService<ActivityType, int, ActivityTypeViewDto, ActivityTypeViewDto, ActivityTypeCreateDto, ActivityTypeCreateDto>,
    IActivityTypesService
{
    public ActivityTypesService(ApplicationDbContext dbContext) : base(dbContext)
    {
    }
}
