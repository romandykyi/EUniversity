using EUniversity.Core.Dtos.University;
using EUniversity.Core.Models.University;
using EUniversity.Core.Services;
using EUniversity.Core.Validation;
using EUniversity.Core.Validation.University;
using FluentValidation.TestHelper;

namespace EUniversity.Tests.Validation.University;

public class StudentGroupValidatorTests : UsersValidatorTests
{
    private StudentGroupDtoValidator _validator;

    private const int TestGroupId = 1;
    private const int NonExistentGroupId = 2;

    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        // Mock validator dependencies
        var existenceChecker = Substitute.For<IEntityExistenceChecker>();
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
        StudentGroupDto dto = new(TestStudentId, TestGroupId);

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
        StudentGroupDto dto = new(studentId, TestGroupId);

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
        StudentGroupDto dto = new(NonExistentUserId, TestGroupId);

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
        StudentGroupDto dto = new(userId, TestGroupId);

        // Act
        var result = await _validator.TestValidateAsync(dto);

        // Assert
        result.ShouldHaveValidationErrorFor(dto => dto.StudentId)
            .WithErrorCode(ValidationErrorCodes.UserIsNotInRole);
    }

    [Test]
    public async Task GroupId_GroupDoesNotExist_FailsWithInvalidForeignKeyError()
    {
        // Arrange
        StudentGroupDto dto = new(TestStudentId, NonExistentGroupId);

        // Act
        var result = await _validator.TestValidateAsync(dto);

        // Assert
        result.ShouldHaveValidationErrorFor(dto => dto.GroupId)
            .WithErrorCode(ValidationErrorCodes.InvalidForeignKey);
    }
}
