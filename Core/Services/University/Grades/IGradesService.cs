using EUniversity.Core.Dtos.University.Grades;
using EUniversity.Core.Models.University.Grades;
using EUniversity.Core.Services;

namespace EUniversity.Infrastructure.Services.University
{
    /// <summary>
    /// Service for grades.
    /// </summary>
    public interface IGradesService :
        ICrudService<Grade, int, GradeViewDto, GradeViewDto, GradeCreateDto, GradeCreateDto>
    {
    }
}
