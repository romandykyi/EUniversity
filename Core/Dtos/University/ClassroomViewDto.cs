namespace EUniversity.Core.Dtos.University;

public record ClassroomViewDto(int Id, string Name,
    DateTimeOffset CreationDate, DateTimeOffset UpdateDate);
