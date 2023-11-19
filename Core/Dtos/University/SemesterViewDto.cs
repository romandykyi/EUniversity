namespace EUniversity.Core.Dtos.University;

public record SemesterViewDto(int Id, string Name,
    DateTimeOffset DateFrom, DateTimeOffset DateTo,
    IEnumerable<StudentSemesterViewDto> StudentEnrollments);
