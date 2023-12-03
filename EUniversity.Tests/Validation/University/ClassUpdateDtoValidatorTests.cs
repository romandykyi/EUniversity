using EUniversity.Core.Dtos.University;
using EUniversity.Core.Services;
using EUniversity.Core.Validation.University;

namespace EUniversity.Tests.Validation.University;

public class ClassUpdateDtoValidatorTests : ClassWriteDtoValidatorTests<ClassUpdateDto>
{
    protected override ClassUpdateDto CreateDto(
        int classroomId = TestIntId, int groupId = TestIntId,
        string? substituteSubstituteTeacherId = TestTeacherId,
        DateTimeOffset? startDate = null, TimeSpan? duration = null)
    {
        return new(classroomId, groupId, substituteSubstituteTeacherId, startDate ?? DateTimeOffset.Now, duration ?? TimeSpan.FromHours(1));
    }

    protected override ClassWriteDtoValidator<ClassUpdateDto> CreateValidator(IEntityExistenceChecker existenceChecker)
    {
        return new ClassUpdateDtoValidator(existenceChecker, UserManagerMock);
    }
}
