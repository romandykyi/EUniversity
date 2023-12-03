using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace EUniversity.Core.Dtos.University;

[ValidateNever] // Remove data annotations validation
public record ClassUpdateDto(int ClassroomId, int GroupId,
    string? SubstituteTeacherId,
    DateTimeOffset StartDate, TimeSpan Duration) : IClassWriteDto;
