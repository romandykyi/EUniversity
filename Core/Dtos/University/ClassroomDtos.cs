using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace EUniversity.Core.Dtos.University;

[ValidateNever] // Remove data annotations validation
public record ClassroomCreateDto(string Name);

public record ClassroomViewDto(int Id, string Name,
    DateTimeOffset CreationDate, DateTimeOffset UpdateDate);

