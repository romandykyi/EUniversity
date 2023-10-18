using EUniversity.Core.Dtos.University;
using EUniversity.Core.Models.University;
using EUniversity.Core.Validation.University;
using EUniversity.Core.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EUniversity.Core.Validation.University.Grades;
using EUniversity.Core.Dtos.University.Grades;
using FluentValidation.TestHelper;
using EUniversity.Core.Models.University.Grades;

namespace EUniversity.Tests.Validation.University.Grades
{
    internal class GradeDtoValidatorTests
    {
        private GradeDtoValidator _validator;

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
            GradeDto dto = new(DefaultName, DefaultScore);

            // Act
            var result = _validator.TestValidate(dto);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }

        [Test]
        public void Name_TooLarge_IsInvalid()
        {
            // Arrange
            GradeDto dto = new(new string('0', Grade.MaxNameLength + 1), DefaultScore);

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
            GradeDto dto = new(string.Empty, DefaultScore);

            // Act
            var result = _validator.TestValidate(dto);

            // Assert
            result.ShouldHaveValidationErrorFor(dto => dto.Name)
                .WithErrorCode(ValidationErrorCodes.PropertyRequired);
        }
    }
}
