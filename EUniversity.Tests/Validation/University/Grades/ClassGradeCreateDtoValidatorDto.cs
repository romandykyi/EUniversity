using EUniversity.Core.Dtos.University.Grades;
using EUniversity.Core.Models.University;
using EUniversity.Core.Models.University.Grades;
using EUniversity.Core.Services;
using EUniversity.Core.Validation;
using EUniversity.Core.Validation.University.Grades;
using FluentValidation.TestHelper;

namespace EUniversity.Tests.Validation.University.Grades;

public class ClassGradeCreateDtoValidatorDto : UsersValidatorTests
{
    private ClassGradeCreateDtoValidator _validator;

    private const int TestGroupId = 1;
    private const int TestGradeId = 2;
    private const int TestActivityTypeId = 3;
    private const int NonExistentActivityTypeId = 4;

    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        // Mock validator dependencies
        var existenceChecker = Substitute.For<IEntityExistenceChecker>();
        existenceChecker
            .ExistsAsync<ActivityType, int>(Arg.Any<int>())
            .Returns(false);
        existenceChecker
            .ExistsAsync<ActivityType, int>(TestActivityTypeId)
            .Returns(true);
        existenceChecker
            .ExistsAsync<Grade, int>(Arg.Any<int>())
            .Returns(true);
        existenceChecker
            .ExistsAsync<Group, int>(Arg.Any<int>())
            .Returns(true);

        _validator = new(existenceChecker, UserManagerMock);
    }

    [Test]
    public async Task ActivityType_Null_Succeeds()
    {
        // Arrange
        ClassGradeCreateDto dto = new(TestGradeId, TestGroupId, TestStudentId, null, null);

        // Act
        var result = await _validator.TestValidateAsync(dto);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Test]
    public async Task ActivityTypeId_ActivityTypeDoesNotExist_FailsWithInvalidForeignKeyError()
    {
        // Arrange
        ClassGradeCreateDto dto = new(
            TestGradeId, TestGroupId, TestStudentId, null, NonExistentActivityTypeId);

        // Act
        var result = await _validator.TestValidateAsync(dto);

        // Assert
        result.ShouldHaveValidationErrorFor(dto => dto.ActivityTypeId!.Value)
            .WithErrorCode(ValidationErrorCodes.InvalidForeignKey);
    }

    [Test]
    public async Task ActivityTypeId_ActivityTypeExists_Succeeds()
    {
        // Arrange
        ClassGradeCreateDto dto = new(
            TestGradeId, TestGroupId, TestStudentId, null, TestActivityTypeId);

        // Act
        var result = await _validator.TestValidateAsync(dto);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }
}
