using EUniversity.Core.Dtos.University;
using EUniversity.Core.Models.University;
using EUniversity.Core.Services;
using EUniversity.Core.Validation;
using EUniversity.Core.Validation.University;
using FluentValidation.TestHelper;

namespace EUniversity.Tests.Validation.University;

public abstract class ClassWriteDtoValidatorTests<TDto> : UsersValidatorTests
    where TDto : IClassWriteDto
{
    protected ClassWriteDtoValidator<TDto> Validator { get; private set; }

    protected const int TestIntId = 5;
    protected const int NonExistentIntId = 4;
    protected const long DefaultDurationTicks = 36_000_000_000L;

    protected abstract TDto CreateDto(int classroomId = TestIntId, int groupId = TestIntId,
        string? substituteSubstituteTeacherId = TestTeacherId, DateTimeOffset? startDate = null, 
        long durationTicks = DefaultDurationTicks);

    protected abstract ClassWriteDtoValidator<TDto> CreateValidator(IEntityExistenceChecker existenceChecker);

    [OneTimeSetUp]
    public virtual void OneTimeSetUp()
    {
        // Mock validator dependencies
        var existenceChecker = Substitute.For<IEntityExistenceChecker>();
        existenceChecker
            .ExistsAsync<Classroom, int>(Arg.Any<int>())
            .Returns(false);
        existenceChecker
            .ExistsAsync<Classroom, int>(TestIntId)
            .Returns(true);
        existenceChecker
            .ExistsAsync<Group, int>(Arg.Any<int>())
            .Returns(false);
        existenceChecker
            .ExistsAsync<Group, int>(TestIntId)
            .Returns(true);

        Validator = CreateValidator(existenceChecker);
    }

    [Test]
    public async Task Dto_Valid_Succeeds()
    {
        // Arrange
        TDto dto = CreateDto();

        // Act
        var result = await Validator.TestValidateAsync(dto);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Test]
    public async Task ClassroomId_ClassroomDoesNotExist_FailsWithInvalidForeignKeyError()
    {
        // Arrange
        TDto dto = CreateDto(classroomId: NonExistentIntId);

        // Act
        var result = await Validator.TestValidateAsync(dto);

        // Assert
        result.ShouldHaveValidationErrorFor(dto => dto.ClassroomId)
            .WithErrorCode(ValidationErrorCodes.InvalidForeignKey);
    }

    [Test]
    public async Task GroupId_GroupDoesNotExist_FailsWithInvalidForeignKeyError()
    {
        // Arrange
        TDto dto = CreateDto(groupId: NonExistentIntId);

        // Act
        var result = await Validator.TestValidateAsync(dto);

        // Assert
        result.ShouldHaveValidationErrorFor(dto => dto.GroupId)
            .WithErrorCode(ValidationErrorCodes.InvalidForeignKey);
    }

    [Test]
    [TestCase("")]
    [TestCase(" ")]
    [TestCase("\t")]
    [TestCase(null)]
    public async Task SubstituteTeacherId_NullOrWhiteSpace_Succeeds(string? substituteTeacherId)
    {
        // Arrange
        TDto dto = CreateDto(substituteSubstituteTeacherId: substituteTeacherId);

        // Act
        var result = await Validator.TestValidateAsync(dto);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Test]
    public async Task SubstituteTeacherId_UserDoesNotExist_FailsWithInvalidForeignKeyError()
    {
        // Arrange
        TDto dto = CreateDto(substituteSubstituteTeacherId: NonExistentUserId);

        // Act
        var result = await Validator.TestValidateAsync(dto);

        // Assert
        result.ShouldHaveValidationErrorFor(dto => dto.SubstituteTeacherId)
            .WithErrorCode(ValidationErrorCodes.InvalidForeignKey);
    }

    [Test]
    public async Task SubstituteTeacherId_UserWithoutTeacherRole_FailsWithUserIsNotInRoleError()
    {
        // Arrange
        TDto dto = CreateDto(substituteSubstituteTeacherId: TestUserId);

        // Act
        var result = await Validator.TestValidateAsync(dto);

        // Assert
        result.ShouldHaveValidationErrorFor(dto => dto.SubstituteTeacherId)
            .WithErrorCode(ValidationErrorCodes.UserIsNotInRole);
    }

    [Test]
    public async Task DurationTicks_Negative_FailsWithInvalidRangeError()
    {
        // Arrange
        TDto dto = CreateDto(durationTicks: -1L);

        // Act
        var result = await Validator.TestValidateAsync(dto);

        // Assert
        result.ShouldHaveValidationErrorFor(dto => dto.DurationTicks)
            .WithErrorCode(ValidationErrorCodes.InvalidRange);
    }
}
