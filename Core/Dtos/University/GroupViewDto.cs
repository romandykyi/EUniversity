using EUniversity.Core.Dtos.Users;

namespace EUniversity.Core.Dtos.University;

public record GroupViewDto(int Id, string Name,
    TeacherPreviewDto Teacher, CoursePreviewDto Course,
    IEnumerable<StudentPreviewDto> Students);
