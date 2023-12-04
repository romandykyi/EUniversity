using EUniversity.Core.Dtos.University;
using EUniversity.Core.Services;
using EUniversity.Core.Validation;
using EUniversity.Core.Validation.University;
using FluentValidation.TestHelper;

namespace EUniversity.Tests.Validation.University;

public class ClassCreateDtoValidatorTests : ClassWriteDtoValidatorTests<ClassCreateDto>
{
    protected override ClassCreateDto CreateDto(
        int classTypeId = TestIntId,
        int classroomId = TestIntId, int groupId = TestIntId, 
        string? substituteSubstituteTeacherId = TestTeacherId, 
        DateTimeOffset? startDate = null, TimeSpan? duration = null)
    {
        return CreateDto(classTypeId, classroomId, groupId,
            substituteSubstituteTeacherId, startDate, duration);
    }

    protected ClassCreateDto CreateDto(
        int classTypeId = TestIntId,
        int classroomId = TestIntId, int groupId = TestIntId, 
        string? substituteSubstituteTeacherId = TestTeacherId, 
        DateTimeOffset? startDate = null, TimeSpan? duration = null,
        int? repeats = null, int? repeatsDelayDays = null)
    {
        return new(classTypeId, classroomId, groupId, 
            substituteSubstituteTeacherId, startDate ?? DateTimeOffset.Now, 
            duration ?? TimeSpan.FromHours(1),
            repeats, repeatsDelayDays);
    }

    protected override ClassWriteDtoValidator<ClassCreateDto> CreateValidator(IEntityExistenceChecker existenceChecker)
    {
        return new ClassCreateDtoValidator(existenceChecker, UserManagerMock);
    }

    [Test]
    [TestCase(-1)]
    [TestCase(0)]
    public async Task Repeats_NonPositive_FailsWithInvalidRangeError(int repeats)
    {
        // Arrange
        ClassCreateDto dto = CreateDto(repeats: repeats);

        // Act
        var result = await Validator.TestValidateAsync(dto);

        // Assert
        result.ShouldHaveValidationErrorFor(dto => dto.Repeats)
            .WithErrorCode(ValidationErrorCodes.InvalidRange);
    }

    [Test]
    [TestCase(-1)]
    [TestCase(0)]
    public async Task RepeatsDelayDays_NonPositive_FailsWithInvalidRangeError(int repeatsDelayDays)
    {
        // Arrange
        ClassCreateDto dto = CreateDto(repeatsDelayDays: repeatsDelayDays, repeats: 5);

        // Act
        var result = await Validator.TestValidateAsync(dto);

        // Assert
        result.ShouldHaveValidationErrorFor(dto => dto.RepeatsDelayDays)
            .WithErrorCode(ValidationErrorCodes.InvalidRange);
    }

    [Test]
    public async Task OnlyRepeatsDelayDays_Null_FailsWithPropertyRequiredError()
    {
        // Arrange
        ClassCreateDto dto = CreateDto(repeatsDelayDays: null, repeats: 5);

        // Act
        var result = await Validator.TestValidateAsync(dto);

        // Assert
        result.ShouldHaveValidationErrorFor(dto => dto.RepeatsDelayDays)
            .WithErrorCode(ValidationErrorCodes.PropertyRequired);
    }

    [Test]
    public async Task OnlyRepeats_Null_FailsWithPropertyRequiredError()
    {
        // Arrange
        ClassCreateDto dto = CreateDto(repeatsDelayDays: 5, repeats: null);

        // Act
        var result = await Validator.TestValidateAsync(dto);

        // Assert
        result.ShouldHaveValidationErrorFor(dto => dto.Repeats)
            .WithErrorCode(ValidationErrorCodes.PropertyRequired);
    }
}
