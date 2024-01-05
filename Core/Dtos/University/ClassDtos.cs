using EUniversity.Core.Dtos.Users;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace EUniversity.Core.Dtos.University;

[ValidateNever] // Remove data annotations validation
public record ClassCreateDto(int ClassTypeId,
    int ClassroomId, int GroupId, string? SubstituteTeacherId,
    DateTimeOffset StartDate, TimeSpan Duration,
    int? Repeats, int? RepeatsDelayDays) : IClassWriteDto;

[ValidateNever] // Remove data annotations validation
public record ClassUpdateDto(int ClassTypeId,
    int ClassroomId, int GroupId, string? SubstituteTeacherId,
    DateTimeOffset StartDate, TimeSpan Duration) : IClassWriteDto;

public record ClassViewDto(int Id,
    DateTimeOffset StartDate, TimeSpan Duration,
    DateTimeOffset CreationDate, DateTimeOffset UpdateDate,
    ClassGroupViewDto Group, ClassClassroomViewDto Classroom,
    ClassClassTypeViewDto ClassType,
    TeacherPreviewDto? SubstituteTeacher);

public record ClassClassroomViewDto(int Id, string Name);

public record ClassClassTypeViewDto(int Id, string Name);

public record ClassGroupViewDto(int Id, string Name,
    TeacherPreviewDto? Teacher, ClassCourseViewDto Course);

public record ClassCourseViewDto(int Id, string Name, SemesterMinimalViewDto? Semester);
