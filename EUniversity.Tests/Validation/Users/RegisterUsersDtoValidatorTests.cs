using EUniversity.Core.Dtos.Auth;
using EUniversity.Core.Dtos.Users;
using EUniversity.Core.Validation;
using EUniversity.Core.Validation.Users;
using FluentValidation.TestHelper;

namespace EUniversity.Tests.Validation.Users
{
    public class RegisterUsersDtoValidatorTests
    {
        private RegisterUsersDtoValidator _validator;
        private readonly RegisterDto _validRegisterDto =
            new("example@email.com", "Joe", "Doe");

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _validator = new();
        }

        [Test]
        public void Dtos_ValidChildren_IsValid()
        {
            // Arrange
            RegisterUsersDto dto = new(new RegisterDto[1] { _validRegisterDto });

            // Act
            var result = _validator.TestValidate(dto);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }

        [Test]
        public void Dtos_Empty_IsInvalid()
        {
            // Arrange
            RegisterUsersDto dto = new(Enumerable.Empty<RegisterDto>());

            // Act
            var result = _validator.TestValidate(dto);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Users)
                .WithErrorCode(ValidationErrorCodes.EmptyCollection)
                .Only();
        }

        [Test]
        public void Dtos_InvalidChildElement_IsInvalid()
        {
            // Arrange
            RegisterUsersDto users = new(
                new RegisterDto[2]
                {
                    _validRegisterDto,
                    new RegisterDto("invalid-email", string.Empty, string.Empty)
                });

            // Act
            var result = _validator.TestValidate(users);

            // Assert
            result.ShouldHaveAnyValidationError();
        }
    }
}
