using EUniversity.Core.Dtos.University.Grades;
using EUniversity.Core.Models.University.Grades;
using EUniversity.Core.Validation;
using EUniversity.Core.Validation.University.Grades;
using FluentValidation.TestHelper;

namespace EUniversity.Tests.Validation.University.Grades
{
    internal class GradeDtoValidatorTests
    {
        private GradeCreateDtoValidator _validator;

        public const int DefaultScore = 5;
        public readonly string DefaultName = "5";

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _validator = new();
        }

        [Test]
        public void Name_Valid_IsValid()
        {
            // Arrange
            GradeCreateDto dto = new(DefaultName, DefaultScore);

            // Act
            var result = _validator.TestValidate(dto);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }

        [Test]
        public void Name_TooLarge_IsInvalid()
        {
            // Arrange
            GradeCreateDto dto = new(new string('0', Grade.MaxNameLength + 1), DefaultScore);

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
            GradeCreateDto dto = new(string.Empty, DefaultScore);

            // Act
            var result = _validator.TestValidate(dto);

            // Assert
            result.ShouldHaveValidationErrorFor(dto => dto.Name)
                .WithErrorCode(ValidationErrorCodes.PropertyRequired);
        }
    }
}
