namespace EUniversity.Core.Dtos.University;

public record CourseMinimalViewDto(int Id, string Name, SemesterMinimalViewDto? Semester);