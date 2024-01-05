using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace EUniversity.Core.Dtos.University;

[ValidateNever] // Remove data annotations validation
public record ActivityTypeCreateDto(string Name);

public record ActivityTypeViewDto(int Id, string Name,
    DateTimeOffset CreationDate, DateTimeOffset UpdateDate);

public record ActivityTypeMinimalDto(int Id, string Name);