namespace EUniversity.Core.Dtos.University;

public record SemesterPreviewDto(int Id, string Name, 
    DateTimeOffset CreationDate, DateTimeOffset UpdateDate,
    DateTimeOffset DateFrom, DateTimeOffset DateTo);
