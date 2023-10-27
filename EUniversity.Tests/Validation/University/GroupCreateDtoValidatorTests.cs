using EUniversity.Core.Dtos.University;
using EUniversity.Core.Models.University;
using EUniversity.Core.Services;
using EUniversity.Core.Validation;
using EUniversity.Core.Validation.University;
using FluentValidation.TestHelper;

namespace EUniversity.Tests.Validation.University;

public class GroupCreateDtoValidatorTests : UsersValidatorTests
{
    private GroupCreateDtoValidator _validator;

    private const string TestGroupName = "977-A";
    private const int TestCourseId = 5;
    private const int NonExistentCourseId = 4;

    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        // Mock validator dependencies
        var existenceChecker = Substitute.For<IEntityExistenceChecker>();
        existenceChecker
            .ExistsAsync<Course, int>(Arg.Any<int>())
            .Returns(false);
        existenceChecker
            .ExistsAsync<Course, int>(TestCourseId)
            .Returns(true);

        _validator = new(existenceChecker, UserManagerMock);
    }

    [Test]
    public async Task Dto_Valid_Succeeds()
    {
        // Arrange
        GroupCreateDto dto = new(TestGroupName, TestCourseId, TestTeacherId);

        // Act
        var result = await _validator.TestValidateAsync(dto);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Test]
    public async Task Name_Empty_FailsWithPropertyRequiredError()
    {
        // Arrange
        GroupCreateDto dto = new(string.Empty, TestCourseId, TestTeacherId);

        // Act
        var result = await _validator.TestValidateAsync(dto);

        // Assert
        result.ShouldHaveValidationErrorFor(dto => dto.Name)
            .WithErrorCode(ValidationErrorCodes.PropertyRequired);
    }

    [Test]
    public async Task Name_TooLarge_FailsWithPropertyTooLargeError()
    {
        // Arrange
        string largeName = new('x', Group.MaxNameLength + 1);
        GroupCreateDto dto = new(largeName, TestCourseId, TestTeacherId);

        // Act
        var result = await _validator.TestValidateAsync(dto);

        // Assert
        result.ShouldHaveValidationErrorFor(dto => dto.Name)
            .WithErrorCode(ValidationErrorCodes.PropertyTooLarge);
    }

    [Test]
    public async Task CourseId_CourseDoesNotExist_FailsWithInvalidForeignKeyError()
    {
        // Arrange
        GroupCreateDto dto = new(TestGroupName, NonExistentCourseId, TestTeacherId);

        // Act
        var result = await _validator.TestValidateAsync(dto);

        // Assert
        result.ShouldHaveValidationErrorFor(dto => dto.CourseId)
            .WithErrorCode(ValidationErrorCodes.InvalidForeignKey);
    }

    [Test]
    [TestCase("")]
    [TestCase(" ")]
    [TestCase("\t")]
    [TestCase(null)]
    public async Task TeacherId_NullOrWhiteSpace_Succeeds(string? teacherId)
    {
        // Arrange
        GroupCreateDto dto = new(TestGroupName, TestCourseId, teacherId);

        // Act
        var result = await _validator.TestValidateAsync(dto);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Test]
    public async Task TeacherId_UserDoesNotExist_FailsWithInvalidForeignKeyError()
    {
        // Arrange
        GroupCreateDto dto = new(TestGroupName, TestCourseId, NonExistentUserId);

        // Act
        var result = await _validator.TestValidateAsync(dto);

        // Assert
        result.ShouldHaveValidationErrorFor(dto => dto.TeacherId)
            .WithErrorCode(ValidationErrorCodes.InvalidForeignKey);
    }

    [Test]
    public async Task TeacherId_UserWithoutTeacherRole_FailsWithUserIsNotInRoleError()
    {
        // Arrange
        GroupCreateDto dto = new(TestGroupName, TestCourseId, TestUserId);

        // Act
        var result = await _validator.TestValidateAsync(dto);

        // Assert
        result.ShouldHaveValidationErrorFor(dto => dto.TeacherId)
            .WithErrorCode(ValidationErrorCodes.UserIsNotInRole);
    }
}
