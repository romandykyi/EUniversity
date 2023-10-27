using EUniversity.Core.Dtos.Users;

namespace EUniversity.Core.Dtos.University;

public record GroupPreviewDto(int Id, string Name, TeacherPreviewDto? Teacher, CoursePreviewDto Course);
