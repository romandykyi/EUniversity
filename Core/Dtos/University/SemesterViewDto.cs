namespace EUniversity.Core.Dtos.University;

public record SemesterViewDto(int Id, string Name,
    DateTimeOffset CreationDate, DateTimeOffset UpdateDate,
    DateTimeOffset DateFrom, DateTimeOffset DateTo,
    IEnumerable<StudentSemesterViewDto> StudentEnrollments);
