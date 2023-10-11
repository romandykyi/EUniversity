using EUniversity.Core.Dtos.University;
using EUniversity.Core.Models.University;
using EUniversity.Core.Validation;
using FluentValidation.TestHelper;

namespace EUniversity.Tests.Validation
{
    public class CreateClassroomDtoValidatorTests
    {
        private CreateClassroomDtoValidator _validator;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _validator = new();
        }

        [Test]
        public void Name_Valid_IsValid()
        {
            // Arrange
            CreateClassromDto dto = new("Room 115");

            // Act
            var result = _validator.TestValidate(dto);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }

        [Test]
        public void Name_TooLarge_IsInvalid()
        {
            // Arrange
            CreateClassromDto dto = new(new string('0', Classroom.MaxNameLength + 1));

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
            CreateClassromDto dto = new(string.Empty);

            // Act
            var result = _validator.TestValidate(dto);

            // Assert
            result.ShouldHaveValidationErrorFor(dto => dto.Name)
                .WithErrorCode(ValidationErrorCodes.PropertyRequired);
        }
    }
}
