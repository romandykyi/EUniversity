using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace EUniversity.Core.Dtos.University;

[ValidateNever] // Remove data annotations validation
public record SemesterCreateDto(string Name, DateTimeOffset DateFrom, DateTimeOffset DateTo);

public record SemesterMinimalViewDto(int Id, string Name);

public record SemesterPreviewDto(int Id, string Name,
    DateTimeOffset CreationDate, DateTimeOffset UpdateDate,
    DateTimeOffset DateFrom, DateTimeOffset DateTo);

public record SemesterViewDto(int Id, string Name,
    DateTimeOffset CreationDate, DateTimeOffset UpdateDate,
    DateTimeOffset DateFrom, DateTimeOffset DateTo);