using EUniversity.Core.Dtos.Auth;
using EUniversity.Core.Validation;
using FluentValidation.TestHelper;

namespace EUniversity.Tests.Validation
{
    public class ChangePasswordDtoValidatorTests
    {
        private ChangePasswordDtoValidator _validator;

        public const string CurrentPassword = "password";
        public const string ValidNewPassword = "Password1!";

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _validator = new ChangePasswordDtoValidator();
        }

        [Test]
        public void Input_Valid_Succeeds()
        {
            // Arrange
            ChangePasswordDto password = new(CurrentPassword, ValidNewPassword);

            // Act
            var result = _validator.TestValidate(password);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }

        [Test]
        public void OldPassword_Empty_Fails()
        {
            // Arrange
            ChangePasswordDto password = new(string.Empty, ValidNewPassword);

            // Act
            var result = _validator.TestValidate(password);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Current)
                .WithErrorCode(ValidationErrorCodes.PropertyRequired)
                .Only();
        }

        [Test]
        public void NewPassword_Empty_Succeeds()
        {
            // Arrange
            ChangePasswordDto password = new(CurrentPassword, string.Empty);

            // Act
            var result = _validator.TestValidate(password);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.New)
                .WithErrorCode(ValidationErrorCodes.PropertyRequired)
                .Only();
        }

        [Test]
        public void Passwords_Empty_FailsWithoutEqualErrorCode()
        {
            // Arrange
            ChangePasswordDto password = new(string.Empty, string.Empty);

            // Act
            var result = _validator.TestValidate(password);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.New)
                .WithoutErrorCode(ValidationErrorCodes.Equal);
        }

        [Test]
        public void Passwords_Equal_Fails()
        {
            // Arrange
            ChangePasswordDto password = new(ValidNewPassword, ValidNewPassword);

            // Act
            var result = _validator.TestValidate(password);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.New)
                .WithErrorCode(ValidationErrorCodes.Equal)
                .Only();
        }
    }
}
