namespace EUniversity.Core.Dtos.University;

public record CourseViewDto(int Id, string Name, string? Description,
    DateTimeOffset CreationDate, DateTimeOffset UpdateDate,
    SemesterPreviewDto? Semester);
