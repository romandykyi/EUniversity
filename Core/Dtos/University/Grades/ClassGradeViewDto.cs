using EUniversity.Core.Dtos.Users;

namespace EUniversity.Core.Dtos.University.Grades;
public record ClassGradeViewDto(int Id, string? Notes,
    GradeMinimalViewDto Grade, GroupMinimalViewDto Group,
    UserViewDto? Assigner, UserViewDto? Reassigner,
    StudentPreviewDto? StudentPreview,
    DateTimeOffset CreationDate, DateTimeOffset UpdateDate,
    ActivityTypeMinimalDto ActivityType
    ) : AssignedGradeViewDto(Id, Notes, Grade, Group, Assigner,
        Reassigner, StudentPreview, CreationDate, UpdateDate);

