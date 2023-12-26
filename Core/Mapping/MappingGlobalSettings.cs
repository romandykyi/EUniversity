using EUniversity.Core.Dtos.University;
using EUniversity.Core.Dtos.University.Grades;
using EUniversity.Core.Models.University;
using EUniversity.Core.Models.University.Grades;
using Mapster;

namespace EUniversity.Core.Mapping;

public static class MappingGlobalSettings
{
    public static void Apply()
    {
        TypeAdapterConfig<Group, GroupPreviewDto>.NewConfig()
            .IgnoreIf((src, dest) => src.TeacherId == null, dest => dest.Teacher!);
        TypeAdapterConfig<Course, CoursePreviewDto>.NewConfig()
            .IgnoreIf((src, dest) => src.SemesterId == null, dest => dest.Semester!);
        TypeAdapterConfig<Course, CourseMinimalViewDto>.NewConfig()
            .IgnoreIf((src, dest) => src.SemesterId == null, dest => dest.Semester!);
        TypeAdapterConfig<Class, ClassViewDto>.NewConfig()
            .IgnoreIf((src, dest) => src.SubstituteTeacher == null, dest => dest.SubstituteTeacher!);
        TypeAdapterConfig<Group, ClassGroupViewDto>.NewConfig()
            .IgnoreIf((src, dest) => src.Teacher == null, dest => dest.Teacher!);
        TypeAdapterConfig<Course, ClassCourseViewDto>.NewConfig()
            .IgnoreIf((src, dest) => src.Semester == null, dest => dest.Semester!);
        TypeAdapterConfig<AssignedGrade, AssignedGradeViewDto>.NewConfig()
            .IgnoreIf((src, dest) => src.ActivityType == null, dest => dest.ActivityType!)
            .IgnoreIf((src, dest) => src.Assigner == null, dest => dest.Assigner!)
            .IgnoreIf((src, dest) => src.Reassigner == null, dest => dest.Reassigner!);

        TypeAdapterConfig.GlobalSettings.Default
            .AddDestinationTransform((string? dest) => string.IsNullOrWhiteSpace(dest) ? null : dest);
    }
}
