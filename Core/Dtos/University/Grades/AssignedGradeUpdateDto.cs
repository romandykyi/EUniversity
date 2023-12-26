using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace EUniversity.Core.Dtos.University.Grades;

[ValidateNever] // Remove data annotations validation
public record class AssignedGradeUpdateDto(int GradeId, string? Notes, int? ActivityTypeId);
