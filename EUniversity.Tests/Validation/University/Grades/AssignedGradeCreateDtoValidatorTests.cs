using EUniversity.Core.Dtos.University.Grades;
using EUniversity.Core.Models.University;
using EUniversity.Core.Models.University.Grades;
using EUniversity.Core.Services;
using EUniversity.Core.Validation;
using EUniversity.Core.Validation.University.Grades;
using FluentValidation.TestHelper;

namespace EUniversity.Tests.Validation.University.Grades;

public class AssignedGradeCreateDtoValidatorTests : UsersValidatorTests
{
    private AssignedGradeCreateDtoValidator<AssignedGradeCreateDto> _validator;

    private const string TestNotes = "Good job!";
    private const int TestGradeId = 1;
    private const int NonExistentGradeId = 2;
    private const int TestGroupId = 5;
    private const int NonExistentGroupId = 4;

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
            .ExistsAsync<Group, int>(Arg.Any<int>())
            .Returns(false);
        existenceChecker
            .ExistsAsync<Group, int>(TestGroupId)
            .Returns(true);

        _validator = new(existenceChecker, UserManagerMock);
    }

    [Test]
    public async Task Dto_Valid_Succeeds()
    {
        // Arrange
        AssignedGradeCreateDto dto = new(TestGradeId, TestGroupId, TestStudentId, TestNotes);

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
        AssignedGradeCreateDto dto = new(TestGradeId, TestGroupId, TestStudentId, notes);

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
        AssignedGradeCreateDto dto = new(TestGradeId, TestGroupId, TestStudentId, largeNotes);

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
        AssignedGradeCreateDto dto = new(NonExistentGradeId, NonExistentGroupId, TestStudentId, TestNotes);

        // Act
        var result = await _validator.TestValidateAsync(dto);

        // Assert
        result.ShouldHaveValidationErrorFor(dto => dto.GradeId)
            .WithErrorCode(ValidationErrorCodes.InvalidForeignKey);
    }

    [Test]
    public async Task GroupId_GroupDoesNotExist_FailsWithInvalidForeignKeyError()
    {
        // Arrange
        AssignedGradeCreateDto dto = new(TestGradeId, NonExistentGroupId, TestStudentId, TestNotes);

        // Act
        var result = await _validator.TestValidateAsync(dto);

        // Assert
        result.ShouldHaveValidationErrorFor(dto => dto.GroupId)
            .WithErrorCode(ValidationErrorCodes.InvalidForeignKey);
    }

    [Test]
    public async Task StudentId_UserDoesNotExist_FailsWithInvalidForeignKeyError()
    {
        // Arrange
        AssignedGradeCreateDto dto = new(TestGradeId, TestGroupId, NonExistentUserId, TestNotes);

        // Act
        var result = await _validator.TestValidateAsync(dto);

        // Assert
        result.ShouldHaveValidationErrorFor(dto => dto.StudentId)
            .WithErrorCode(ValidationErrorCodes.InvalidForeignKey);
    }

    [Test]
    public async Task StudentId_UserWithoutStudentRole_FailsWithUserIsNotInRoleError()
    {
        // Arrange
        AssignedGradeCreateDto dto = new(TestGradeId, TestGroupId, TestUserId, TestNotes);

        // Act
        var result = await _validator.TestValidateAsync(dto);

        // Assert
        result.ShouldHaveValidationErrorFor(dto => dto.StudentId)
            .WithErrorCode(ValidationErrorCodes.UserIsNotInRole);
    }
}
