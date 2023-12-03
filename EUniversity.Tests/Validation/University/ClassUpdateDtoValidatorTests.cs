using EUniversity.Core.Dtos.University;
using EUniversity.Core.Services;
using EUniversity.Core.Validation.University;

namespace EUniversity.Tests.Validation.University;

public class ClassUpdateDtoValidatorTests : ClassWriteDtoValidatorTests<ClassUpdateDto>
{
    protected override ClassUpdateDto CreateDto(
        int classroomId = TestIntId, int groupId = TestIntId,
        string? substituteSubstituteTeacherId = TestTeacherId,
        DateTimeOffset? startDate = null, long durationTicks = DefaultDurationTicks)
    {
        return new(classroomId, groupId, substituteSubstituteTeacherId, startDate ?? DateTimeOffset.Now, durationTicks);
    }

    protected override ClassWriteDtoValidator<ClassUpdateDto> CreateValidator(IEntityExistenceChecker existenceChecker)
    {
        return new ClassUpdateDtoValidator(existenceChecker, UserManagerMock);
    }
}
