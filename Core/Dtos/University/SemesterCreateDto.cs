using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace EUniversity.Core.Dtos.University;

[ValidateNever] // Remove data annotations validation
public record SemesterCreateDto(string Name, DateTimeOffset DateFrom, DateTimeOffset DateTo);
