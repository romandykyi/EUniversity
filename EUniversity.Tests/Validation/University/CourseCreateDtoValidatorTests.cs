using EUniversity.Core.Dtos.University;
using EUniversity.Core.Models.University;
using EUniversity.Core.Services;
using EUniversity.Core.Validation;
using EUniversity.Core.Validation.University;
using FluentValidation.TestHelper;

namespace EUniversity.Tests.Validation.University;

public class CourseCreateDtoValidatorTests : UsersValidatorTests
{
    private CourseCreateDtoValidator _validator;

    private const string DefaultName = "Course";
    private const string DefaultDescription = "Course description. . .";
    private const int TestSemesterId = 4;
    private const int NonExistentSemesterId = 2;

    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        // Mock validator dependencies
        var existenceChecker = Substitute.For<IEntityExistenceChecker>();
        existenceChecker
            .ExistsAsync<Semester, int>(Arg.Any<int>())
            .Returns(false);
        existenceChecker
            .ExistsAsync<Semester, int>(TestSemesterId)
            .Returns(true);

        _validator = new(existenceChecker);
    }

    [Test]
    public async Task Dto_Valid_Succeeds()
    {
        // Arrange
        CourseCreateDto dto = new(DefaultName, DefaultDescription, TestSemesterId);

        // Act
        var result = await _validator.TestValidateAsync(dto);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Test]
    public async Task Name_TooLarge_FailsWithPropertyTooLargeError()
    {
        // Arrange
        CourseCreateDto dto = new(new string('0', Course.MaxNameLength + 1), DefaultDescription,TestSemesterId);

        // Act
        var result = await _validator.TestValidateAsync(dto);

        // Assert
        result.ShouldHaveValidationErrorFor(dto => dto.Name)
            .WithErrorCode(ValidationErrorCodes.PropertyTooLarge);
    }

    [Test]
    public async Task Name_Empty_FailsWithPropertyRequiredError()
    {
        // Arrange
        CourseCreateDto dto = new(string.Empty, DefaultDescription, TestSemesterId);

        // Act
        var result = await _validator.TestValidateAsync(dto);

        // Assert
        result.ShouldHaveValidationErrorFor(dto => dto.Name)
            .WithErrorCode(ValidationErrorCodes.PropertyRequired);
    }

    [Test]
    [TestCase(null)]
    [TestCase("")]
    public async Task Description_NullOrEmpty_Succeeds(string? description)
    {
        // Arrange
        CourseCreateDto dto = new(DefaultName, description, TestSemesterId);

        // Act
        var result = await _validator.TestValidateAsync(dto);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Test]
    public async Task Description_TooLarge_FailsWithPropertyTooLargeError()
    {
        // Arrange
        CourseCreateDto dto = new(DefaultName, new string('0', Course.MaxDescriptionLength + 1), TestSemesterId);

        // Act
        var result = await _validator.TestValidateAsync(dto);

        // Assert
        result.ShouldHaveValidationErrorFor(dto => dto.Description)
            .WithErrorCode(ValidationErrorCodes.PropertyTooLarge);
    }

    [Test]
    public async Task SemesterId_Null_Succeeds()
    {
        // Arrange
        CourseCreateDto dto = new(DefaultName, DefaultDescription, null);

        // Act
        var result = await _validator.TestValidateAsync(dto);

        // Assert
        result.ShouldNotHaveValidationErrorFor(dto => dto.SemesterId);
    }

    [Test]
    public async Task SemesterId_SemesterDoesNotExist_FailsWithInvalidForeignKeyError()
    {
        // Arrange
        CourseCreateDto dto = new(DefaultName, DefaultDescription, NonExistentSemesterId);

        // Act
        var result = await _validator.TestValidateAsync(dto);

        // Assert
        result.ShouldHaveValidationErrorFor(dto => dto.SemesterId)
            .WithErrorCode(ValidationErrorCodes.InvalidForeignKey);
    }
}
