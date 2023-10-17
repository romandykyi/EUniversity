using EUniversity.Core.Dtos.University;
using EUniversity.Core.Models.University;
using EUniversity.Core.Services;

namespace EUniversity.Infrastructure.Services.University
{
    /// <summary>
    /// Service for classrooms.
    /// </summary>
    public interface IClassroomsService :
        ICrudService<Classroom, int, ViewClassroomDto, ViewClassroomDto, CreateClassromDto, CreateClassromDto>
    {
    }
}
