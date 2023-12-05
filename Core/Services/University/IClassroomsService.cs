using EUniversity.Core.Dtos.University;
using EUniversity.Core.Models.University;

namespace EUniversity.Core.Services.University;

/// <summary>
/// Service for classrooms.
/// </summary>
public interface IClassroomsService :
    ICrudService<Classroom, int, ClassroomViewDto, ClassroomViewDto, ClassroomCreateDto, ClassroomCreateDto>
{
}
