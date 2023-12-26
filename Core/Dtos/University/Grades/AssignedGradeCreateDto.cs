using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace EUniversity.Core.Dtos.University.Grades;

[ValidateNever] // Remove data annotations validation
public record AssignedGradeCreateDto(int GradeId, int GroupId, string StudentId, string? Notes, int? ActivityTypeId);
