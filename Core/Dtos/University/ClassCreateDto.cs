using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace EUniversity.Core.Dtos.University;

[ValidateNever] // Remove data annotations validation
public record ClassCreateDto(int ClassroomId, int GroupId,
    string? SubstituteTeacherId,
    DateTimeOffset StartDate, long DurationTicks) : IClassWriteDto;
