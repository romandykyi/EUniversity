using EUniversity.Core.Dtos.University;
using EUniversity.Core.Models.University;
using EUniversity.Core.Validation;
using EUniversity.Core.Validation.University;
using FluentValidation.TestHelper;

namespace EUniversity.Tests.Validation.University;

public class ClassroomCreateDtoValidatorTests
{
    private ClassroomCreateDtoValidator _validator;

    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        _validator = new();
    }

    [Test]
    public void Name_Valid_IsValid()
    {
        // Arrange
        ClassroomCreateDto dto = new("Room 115");

        // Act
        var result = _validator.TestValidate(dto);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Test]
    public void Name_TooLarge_IsInvalid()
    {
        // Arrange
        ClassroomCreateDto dto = new(new string('0', Classroom.MaxNameLength + 1));

        // Act
        var result = _validator.TestValidate(dto);

        // Assert
        result.ShouldHaveValidationErrorFor(dto => dto.Name)
            .WithErrorCode(ValidationErrorCodes.PropertyTooLarge);
    }

    [Test]
    public void Name_Empty_IsInvalid()
    {
        // Arrange
        ClassroomCreateDto dto = new(string.Empty);

        // Act
        var result = _validator.TestValidate(dto);

        // Assert
        result.ShouldHaveValidationErrorFor(dto => dto.Name)
            .WithErrorCode(ValidationErrorCodes.PropertyRequired);
    }
}
