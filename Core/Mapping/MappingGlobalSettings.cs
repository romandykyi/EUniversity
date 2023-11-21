using EUniversity.Core.Dtos.University;
using Mapster;
using EUniversity.Core.Models.University;

namespace EUniversity.Core.Mapping;

public static class MappingGlobalSettings
{
    public static void Apply()
    {
        TypeAdapterConfig<Group, GroupPreviewDto>.NewConfig()
            .IgnoreIf((src, dest) => src.TeacherId == null, dest => dest.Teacher!);
        TypeAdapterConfig<Course, CoursePreviewDto>.NewConfig()
            .IgnoreIf((src, dest) => src.SemesterId == null, dest => dest.Semester!);

        TypeAdapterConfig.GlobalSettings.Default
            .AddDestinationTransform((string? dest) => string.IsNullOrWhiteSpace(dest) ? null : dest);
    }
}
