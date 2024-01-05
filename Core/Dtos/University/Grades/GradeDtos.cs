using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace EUniversity.Core.Dtos.University.Grades;

[ValidateNever] // Remove data annotations validation
public record GradeCreateDto(string Name, int Score);

public record GradeMinimalViewDto(int Id, string Name);

public record GradeViewDto(int Id, string Name, int Score,
    DateTimeOffset CreationDate, DateTimeOffset UpdateDate);
