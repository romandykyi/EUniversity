using EUniversity.Core.Dtos.Users;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace EUniversity.Core.Dtos.University.Grades;

[ValidateNever] // Remove data annotations validation
public record AssignedGradeCreateDto(int GradeId, int GroupId, string StudentId, string? Notes, int? ActivityTypeId);


[ValidateNever] // Remove data annotations validation
public record class AssignedGradeUpdateDto(int GradeId, string? Notes, int? ActivityTypeId);

public record AssignedGradeViewDto(int Id, string? Notes,
    GradeMinimalViewDto Grade, GroupMinimalViewDto? Group,
    UserViewDto? Assigner, UserViewDto? Reassigner,
    StudentPreviewDto? Student,
    DateTimeOffset CreationDate, DateTimeOffset UpdateDate,
    ActivityTypeMinimalDto? ActivityType);