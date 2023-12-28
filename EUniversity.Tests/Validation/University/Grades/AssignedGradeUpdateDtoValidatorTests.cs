using EUniversity.Core.Dtos.University.Grades;
using EUniversity.Core.Models.University;
using EUniversity.Core.Models.University.Grades;
using EUniversity.Core.Services;
using EUniversity.Core.Validation;
using EUniversity.Core.Validation.University.Grades;
using FluentValidation.TestHelper;

namespace EUniversity.Tests.Validation.University.Grades;

public class AssignedGradeUpdateDtoValidatorTests : UsersValidatorTests
{
    private AssignedGradeUpdateDtoValidator _validator;

    private const string TestNotes = "Good job!";
    private const int TestGradeId = 1;
    private const int NonExistentGradeId = 2;
    private const int TestGroupId = 5;
    private const int NonExistentGroupId = 4;
    private const int TestActivityTypeId = 3;
    private const int NonExistentActivityTypeId = 4;

    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        // Mock validator dependencies
        var existenceChecker = Substitute.For<IEntityExistenceChecker>();
        existenceChecker
            .ExistsAsync<Grade, int>(Arg.Any<int>())
            .Returns(false);
        existenceChecker
            .ExistsAsync<Grade, int>(TestGradeId)
            .Returns(true);
        existenceChecker
            .ExistsAsync<ActivityType, int>(Arg.Any<int>())
            .Returns(false);
        existenceChecker
            .ExistsAsync<ActivityType, int>(TestActivityTypeId)
            .Returns(true);

        _validator = new(existenceChecker);
    }

    [Test]
    public async Task Dto_Valid_Succeeds()
    {
        // Arrange
        AssignedGradeUpdateDto dto = new(TestGradeId, TestNotes, TestActivityTypeId);

        // Act
        var result = await _validator.TestValidateAsync(dto);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Test]
    [TestCase(null)]
    [TestCase("")]
    [TestCase(" ")]
    public async Task Notes_EmptyOrNull_Succeeds(string? notes)
    {
        // Arrange
        AssignedGradeUpdateDto dto = new(TestGradeId, notes, TestActivityTypeId);

        // Act
        var result = await _validator.TestValidateAsync(dto);

        // Assert
        result.ShouldNotHaveValidationErrorFor(dto => dto.Notes);
    }

    [Test]
    public async Task Notes_TooLarge_FailsWithPropertyTooLargeError()
    {
        // Arrange
        string largeNotes = new('x', AssignedGrade.MaxNotesLength + 1);
        AssignedGradeUpdateDto dto = new(TestGradeId, largeNotes, TestActivityTypeId);

        // Act
        var result = await _validator.TestValidateAsync(dto);

        // Assert
        result.ShouldHaveValidationErrorFor(dto => dto.Notes)
            .WithErrorCode(ValidationErrorCodes.PropertyTooLarge);
    }

    [Test]
    public async Task GradeId_GradeDoesNotExist_FailsWithInvalidForeignKeyError()
    {
        // Arrange
        AssignedGradeUpdateDto dto = new(NonExistentGradeId, TestNotes, TestActivityTypeId);

        // Act
        var result = await _validator.TestValidateAsync(dto);

        // Assert
        result.ShouldHaveValidationErrorFor(dto => dto.GradeId)
            .WithErrorCode(ValidationErrorCodes.InvalidForeignKey);
    }

    [Test]
    public async Task ActivityType_Null_Succeeds()
    {
        // Arrange
        AssignedGradeUpdateDto dto = new(TestGradeId, null, null);

        // Act
        var result = await _validator.TestValidateAsync(dto);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Test]
    public async Task ActivityTypeId_ActivityTypeDoesNotExist_FailsWithInvalidForeignKeyError()
    {
        // Arrange
        AssignedGradeUpdateDto dto = new(TestGradeId, null, NonExistentActivityTypeId);

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
        AssignedGradeUpdateDto dto = new(TestGradeId, null, TestActivityTypeId);

        // Act
        var result = await _validator.TestValidateAsync(dto);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }
}
