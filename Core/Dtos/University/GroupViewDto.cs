using EUniversity.Core.Dtos.Users;

namespace EUniversity.Core.Dtos.University;

public record GroupViewDto(int Id, string Name,
    DateTimeOffset CreationDate, DateTimeOffset UpdateDate,
    TeacherPreviewDto? Teacher, CoursePreviewDto Course,
    IEnumerable<StudentPreviewDto> Students);
