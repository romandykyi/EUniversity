using EUniversity.Core.Dtos.University;
using EUniversity.Core.Models.University;
using EUniversity.Core.Services;
using EUniversity.Core.Validation;
using EUniversity.Core.Validation.University;
using FluentValidation.TestHelper;

namespace EUniversity.Tests.Validation.University;

public class StudentGroupCreateDtoValidatorTests : UsersValidatorTests
{
    private StudentGroupCreateDtoValidator _validator;

    private const int TestGroupId = 1;
    private const int NonExistentGroupId = 2;

    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        _validator = new(UserManagerMock);
    }

    [Test]
    public async Task Dto_Valid_Succeeds()
    {
        // Arrange
        StudentGroupCreateDto dto = new(TestStudentId);

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
        StudentGroupCreateDto dto = new(studentId);

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
        StudentGroupCreateDto dto = new(NonExistentUserId);

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
        StudentGroupCreateDto dto = new(userId);

        // Act
        var result = await _validator.TestValidateAsync(dto);

        // Assert
        result.ShouldHaveValidationErrorFor(dto => dto.StudentId)
            .WithErrorCode(ValidationErrorCodes.UserIsNotInRole);
    }
}
