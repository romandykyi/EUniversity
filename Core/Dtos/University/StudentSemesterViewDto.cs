using EUniversity.Core.Dtos.Users;

namespace EUniversity.Core.Dtos.University;

public record StudentSemesterViewDto(StudentPreviewDto Student, DateTimeOffset EnrollmentDate);
