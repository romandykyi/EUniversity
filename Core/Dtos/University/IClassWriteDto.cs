namespace EUniversity.Core.Dtos.University;

public interface IClassWriteDto
{
    public int ClassroomId { get; }
    public int GroupId { get; }
    public string? SubstituteTeacherId { get; }

    public DateTimeOffset StartDate { get; }

    public long DurationTicks { get; }
}
