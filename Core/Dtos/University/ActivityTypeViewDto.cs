namespace EUniversity.Core.Dtos.University;

public record ActivityTypeViewDto(int Id, string Name,
    DateTimeOffset CreationDate, DateTimeOffset UpdateDate);