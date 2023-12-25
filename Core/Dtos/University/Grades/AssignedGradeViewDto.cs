using EUniversity.Core.Dtos.Users;

namespace EUniversity.Core.Dtos.University.Grades;

public record AssignedGradeViewDto(int Id, string? Notes, 
    GradeMinimalViewDto Grade, GroupMinimalViewDto? Group,
    UserViewDto? Assigner, UserViewDto? Reassigner,
    StudentPreviewDto? StudentPreview,
    DateTimeOffset CreationDate, DateTimeOffset UpdateDate,
    ActivityTypeMinimalDto? ActivityType);
