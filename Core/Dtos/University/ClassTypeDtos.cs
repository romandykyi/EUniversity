using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace EUniversity.Core.Dtos.University;

public record ClassTypeViewDto(int Id, string Name,
    DateTimeOffset CreationDate, DateTimeOffset UpdateDate);

[ValidateNever] // Remove data annotations validation
public record ClassTypeCreateDto(string Name);
