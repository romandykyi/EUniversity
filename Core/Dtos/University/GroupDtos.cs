using EUniversity.Core.Dtos.Users;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace EUniversity.Core.Dtos.University;

[ValidateNever] // Remove data annotations validation
public record GroupCreateDto(string Name, int CourseId, string? TeacherId);

public record GroupMinimalViewDto(int Id, string Name, CourseMinimalViewDto Course);

public record GroupPreviewDto(int Id, string Name, DateTimeOffset CreationDate, DateTimeOffset UpdateDate, TeacherPreviewDto? Teacher, CoursePreviewDto Course);

public record GroupViewDto(int Id, string Name,
    DateTimeOffset CreationDate, DateTimeOffset UpdateDate,
    TeacherPreviewDto? Teacher, CoursePreviewDto Course,
    IEnumerable<StudentPreviewDto> Students);
