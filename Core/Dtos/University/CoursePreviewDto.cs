namespace EUniversity.Core.Dtos.University;

public record CoursePreviewDto(int Id, string Name,
    DateTimeOffset CreationDate, DateTimeOffset UpdateDate,
    SemesterMinimalViewDto? Semester);
