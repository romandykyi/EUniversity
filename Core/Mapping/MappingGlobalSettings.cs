using EUniversity.Core.Dtos.University;
using EUniversity.Core.Models.University;
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
        TypeAdapterConfig<Class, ClassViewDto>.NewConfig()
            .IgnoreIf((src, dest) => src.SubstituteTeacher == null, dest => dest.SubstituteTeacher!);
        TypeAdapterConfig<Group, ClassGroupViewDto>.NewConfig()
            .IgnoreIf((src, dest) => src.Teacher == null, dest => dest.Teacher!);
        TypeAdapterConfig<Course, ClassCourseViewDto>.NewConfig()
            .IgnoreIf((src, dest) => src.Semester == null, dest => dest.Semester!);

        TypeAdapterConfig.GlobalSettings.Default
            .AddDestinationTransform((string? dest) => string.IsNullOrWhiteSpace(dest) ? null : dest);
    }
}
