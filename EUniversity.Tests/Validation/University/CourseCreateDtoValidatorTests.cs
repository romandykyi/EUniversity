using EUniversity.Core.Dtos.University;
using EUniversity.Core.Models.University;
using EUniversity.Core.Validation;
using EUniversity.Core.Validation.University;
using FluentValidation.TestHelper;

namespace EUniversity.Tests.Validation.University;

public class CourseCreateDtoValidatorTests
{
    private CourseCreateDtoValidator _validator;

    private const string DefaultName = "Course";
    private const string DefaultDescription = "Course description. . .";

    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        _validator = new();
    }

    [Test]
    public void Dto_Valid_Succeeds()
    {
        // Arrange
        CourseCreateDto dto = new(DefaultName, DefaultDescription);

        // Act
        var result = _validator.TestValidate(dto);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Test]
    public void Name_TooLarge_FailsWithPropertyTooLargeError()
    {
        // Arrange
        CourseCreateDto dto = new(new string('0', Course.MaxNameLength + 1), DefaultDescription);

        // Act
        var result = _validator.TestValidate(dto);

        // Assert
        result.ShouldHaveValidationErrorFor(dto => dto.Name)
            .WithErrorCode(ValidationErrorCodes.PropertyTooLarge);
    }

    [Test]
    public void Name_Empty_FailsWithPropertyRequiredError()
    {
        // Arrange
        CourseCreateDto dto = new(string.Empty, DefaultDescription);

        // Act
        var result = _validator.TestValidate(dto);

        // Assert
        result.ShouldHaveValidationErrorFor(dto => dto.Name)
            .WithErrorCode(ValidationErrorCodes.PropertyRequired);
    }

    [Test]
    [TestCase(null)]
    [TestCase("")]
    public void Description_NullOrEmpty_Succeeds(string? description)
    {
        // Arrange
        CourseCreateDto dto = new(DefaultName, description);

        // Act
        var result = _validator.TestValidate(dto);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Test]
    public void Description_TooLarge_FailsWithPropertyTooLargeError()
    {
        // Arrange
        CourseCreateDto dto = new(DefaultName, new string('0', Course.MaxDescriptionLength + 1));

        // Act
        var result = _validator.TestValidate(dto);

        // Assert
        result.ShouldHaveValidationErrorFor(dto => dto.Description)
            .WithErrorCode(ValidationErrorCodes.PropertyTooLarge);
    }
}
