using EUniversity.Core.Dtos.University;
using EUniversity.Core.Models.University;
using EUniversity.Core.Validation;
using EUniversity.Core.Validation.University;
using FluentValidation.TestHelper;

namespace EUniversity.Tests.Validation.University;

public class ActivityTypeCreateDtoValidatorTests
{
    private ActivityTypeCreateDtoValidator _validator;

    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        _validator = new();
    }

    [Test]
    public void Dto_Valid_Succeeds()
    {
        // Arrange
        ActivityTypeCreateDto dto = new("Test");

        // Act
        var result = _validator.TestValidate(dto);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Test]
    public void Name_TooLarge_FailsWithPropertyTooLargeError()
    {
        // Arrange
        ActivityTypeCreateDto dto = new(new string('0', ActivityType.MaxNameLength + 1));

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
        ActivityTypeCreateDto dto = new(string.Empty);

        // Act
        var result = _validator.TestValidate(dto);

        // Assert
        result.ShouldHaveValidationErrorFor(dto => dto.Name)
            .WithErrorCode(ValidationErrorCodes.PropertyRequired);
    }
}
