using EUniversity.Core.Dtos.Users;

namespace EUniversity.Core.Dtos.University;

public record ClassViewDto(int Id,
    DateTimeOffset StartDate, TimeSpan Duration,
    DateTimeOffset CreationDate, DateTimeOffset UpdateDate,
    ClassGroupViewDto Group,
    TeacherPreviewDto? SubstituteTeacher);

public record ClassGroupViewDto(int Id, string Name,
    TeacherPreviewDto? Teacher, ClassCourseViewDto Course);

public record ClassCourseViewDto(int Id, string Name, SemesterMinimalViewDto? Semester);