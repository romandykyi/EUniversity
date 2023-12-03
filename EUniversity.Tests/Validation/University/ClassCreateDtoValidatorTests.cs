using EUniversity.Core.Dtos.University;
using EUniversity.Core.Services;
using EUniversity.Core.Validation.University;

namespace EUniversity.Tests.Validation.University;

public class ClassCreateDtoValidatorTests : ClassWriteDtoValidatorTests<ClassCreateDto>
{
    protected override ClassCreateDto CreateDto(
        int classroomId = TestIntId, int groupId = TestIntId, 
        string? substituteSubstituteTeacherId = TestTeacherId, 
        DateTimeOffset? startDate = null, TimeSpan? duration = null)
    {
        return new(classroomId, groupId, substituteSubstituteTeacherId, startDate ?? DateTimeOffset.Now, duration ?? TimeSpan.FromHours(1));
    }

    protected override ClassWriteDtoValidator<ClassCreateDto> CreateValidator(IEntityExistenceChecker existenceChecker)
    {
        return new ClassCreateDtoValidator(existenceChecker, UserManagerMock);
    }
}
