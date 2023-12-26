using EUniversity.Core.Dtos.University;
using EUniversity.Core.Models.University;

namespace EUniversity.Core.Services.University;

/// <summary>
/// Service for activity types.
/// </summary>
public interface IActivityTypesService :
    ICrudService<ActivityType, int, ActivityTypeViewDto, ActivityTypeViewDto, ActivityTypeCreateDto, ActivityTypeCreateDto>
{
}
