using EUniversity.Core.Dtos.Users;

namespace EUniversity.Core.Dtos.University;

public record StudentGroupViewDto(StudentPreviewDto Student, DateTimeOffset EnrollmentDate);