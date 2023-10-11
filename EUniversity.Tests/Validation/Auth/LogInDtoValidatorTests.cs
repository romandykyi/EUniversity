using EUniversity.Core.Dtos.Auth;
using EUniversity.Core.Validation;
using EUniversity.Core.Validation.Auth;
using FluentValidation.TestHelper;

namespace EUniversity.Tests.Validation.Auth
{
    public class LogInDtoValidatorTests
    {
        private LogInDtoValidator _validator;
        public const string DefaultUserName = "user";
        public const string DefaultPassword = "Passw0rd123";

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _validator = new LogInDtoValidator();
        }

        [Test]
        public void Input_Valid_IsValid()
        {
            // Arrange
            LogInDto login = new(DefaultUserName, DefaultPassword, false);

            // Act
            var result = _validator.TestValidate(login);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }

        [Test]
        public void UserName_Empty_IsInvalid()
        {
            // Arrange
            LogInDto login = new(string.Empty, DefaultPassword, false);

            // Act
            var result = _validator.TestValidate(login);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.UserName)
                .WithErrorCode(ValidationErrorCodes.PropertyRequired)
                .Only();
        }

        [Test]
        public void Password_Empty_IsInvalid()
        {
            // Arrange
            LogInDto login = new(DefaultUserName, string.Empty, false);

            // Act
            var result = _validator.TestValidate(login);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Password)
                .WithErrorCode(ValidationErrorCodes.PropertyRequired)
                .Only();
        }
    }
}
