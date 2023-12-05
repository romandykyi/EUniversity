using EUniversity.Core.Dtos.University;
using EUniversity.Core.Models.University;
using EUniversity.Core.Validation;
using EUniversity.Core.Validation.University;
using FluentValidation.TestHelper;

namespace EUniversity.Tests.Validation.University;

public class ClassTypeCreateDtoValidatorTests
{
    private ClassTypeCreateDtoValidator _validator;

    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        _validator = new();
    }

    [Test]
    public void Dto_Valid_Succeeds()
    {
        // Arrange
        ClassTypeCreateDto dto = new("Lecture");

        // Act
        var result = _validator.TestValidate(dto);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Test]
    public void Name_TooLarge_FailsWithPropertyTooLargeError()
    {
        // Arrange
        ClassTypeCreateDto dto = new(new string('0', ClassType.MaxNameLength + 1));

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
        ClassTypeCreateDto dto = new(string.Empty);

        // Act
        var result = _validator.TestValidate(dto);

        // Assert
        result.ShouldHaveValidationErrorFor(dto => dto.Name)
            .WithErrorCode(ValidationErrorCodes.PropertyRequired);
    }
}
