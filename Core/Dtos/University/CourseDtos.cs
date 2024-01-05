using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace EUniversity.Core.Dtos.University;

[ValidateNever] // Remove data annotations validation
public record CourseCreateDto(string Name, string? Description, int? SemesterId);

public record CourseMinimalViewDto(int Id, string Name, SemesterMinimalViewDto? Semester);

public record CoursePreviewDto(int Id, string Name,
    DateTimeOffset CreationDate, DateTimeOffset UpdateDate,
    SemesterMinimalViewDto? Semester);

public record CourseViewDto(int Id, string Name, string? Description,
    DateTimeOffset CreationDate, DateTimeOffset UpdateDate,
    SemesterPreviewDto? Semester);