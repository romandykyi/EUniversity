using EUniversity.Core.Dtos.University;
using EUniversity.Core.Validation;
using EUniversity.Core.Validation.University;
using FluentValidation.TestHelper;

namespace EUniversity.Tests.Validation.University;

public class AssignStudentDtoValidatorTests : UsersValidatorTests
{
    private AssignStudentDtoValidator _validator;

    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        _validator = new(UserManagerMock);
    }

    [Test]
    public async Task Dto_Valid_Succeeds()
    {
        // Arrange
        AssignStudentDto dto = new(TestStudentId);

        // Act
        var result = await _validator.TestValidateAsync(dto);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Test]
    [TestCase("")]
    [TestCase(" ")]
    [TestCase("\t")]
    public async Task StudentId_Empty_FailsWithPropertyRequiredError(string studentId)
    {
        // Arrange
        AssignStudentDto dto = new(studentId);

        // Act
        var result = await _validator.TestValidateAsync(dto);

        // Assert
        result.ShouldHaveValidationErrorFor(sg => sg.StudentId)
            .WithErrorCode(ValidationErrorCodes.PropertyRequired);
    }

    [Test]
    public async Task StudentId_UserDoesNotExist_FailsWithInvalidForeignKeyError()
    {
        // Arrange
        AssignStudentDto dto = new(NonExistentUserId);

        // Act
        var result = await _validator.TestValidateAsync(dto);

        // Assert
        result.ShouldHaveValidationErrorFor(dto => dto.StudentId)
            .WithErrorCode(ValidationErrorCodes.InvalidForeignKey);
    }

    [Test]
    [TestCase(TestUserId)]
    [TestCase(TestTeacherId)]
    public async Task StudentId_UserWithoutStudentRole_FailsWithUserIsNotInRoleError(string userId)
    {
        // Arrange
        AssignStudentDto dto = new(userId);

        // Act
        var result = await _validator.TestValidateAsync(dto);

        // Assert
        result.ShouldHaveValidationErrorFor(dto => dto.StudentId)
            .WithErrorCode(ValidationErrorCodes.UserIsNotInRole);
    }
}
