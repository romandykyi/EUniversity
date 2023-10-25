using EUniversity.Core.Dtos.University;
using EUniversity.Core.Models.University;
using EUniversity.Infrastructure.Data;

namespace EUniversity.Infrastructure.Services.University
{
    /// <inheritdoc />
    public class ClassroomsService :
        BaseCrudService<Classroom, int, ClassroomViewDto, ClassroomViewDto, ClassroomCreateDto, ClassroomCreateDto>,
        IClassroomsService
    {
        public ClassroomsService(ApplicationDbContext dbContext) : base(dbContext) { }
    }
}
