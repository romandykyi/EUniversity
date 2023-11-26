using EUniversity.Core.Dtos.Users;

namespace EUniversity.Core.Dtos.University;

public record GroupPreviewDto(int Id, string Name, DateTimeOffset CreationDate, DateTimeOffset UpdateDate, TeacherPreviewDto? Teacher, CoursePreviewDto Course);
