using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace EUniversity.Core.Dtos.University;

[ValidateNever] // Remove data annotations validation
public record ClassCreateDto(int ClassTypeId,
    int ClassroomId, int GroupId, string? SubstituteTeacherId,
    DateTimeOffset StartDate, TimeSpan Duration,
    int? Repeats, int? RepeatsDelayDays) : IClassWriteDto;
