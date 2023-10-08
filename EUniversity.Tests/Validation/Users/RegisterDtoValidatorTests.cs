using EUniversity.Core.Dtos.Auth;
using EUniversity.Core.Models;
using EUniversity.Core.Validation;
using EUniversity.Core.Validation.Users;
using FluentValidation.TestHelper;

namespace EUniversity.Tests.Validation.Users
{
    public class RegisterDtoValidatorTests
    {
        private RegisterDtoValidator _validator;

        public const string DefaultEmail = "heisenberg@blue-crystals.com";
        public const string DefaultFirstName = "Walter";
        public const string DefaultLastName = "White";
        public const string DefaultMiddleName = "Hartwell";

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _validator = new RegisterDtoValidator();
        }

        [Test]
        [TestCase("example@email")]
        [TestCase("12345@e-mail.com")]
        [TestCase("mail@example.co.uk")]
        [TestCase("user.name+tag@mail.com")]
        public void Email_Valid_Succeeds(string email)
        {
            // Arrange
            RegisterDto register = new(email, DefaultFirstName, DefaultLastName, DefaultMiddleName);

            // Act
            var result = _validator.TestValidate(register);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }

        [Test]
        public void Email_TooLarge_Fails()
        {
            // Arrange
            string bigString = new('a', ApplicationUser.MaxEmailLength);
            string bigEmail = $"{bigString}@email.com";
            RegisterDto register = new(bigEmail, DefaultFirstName, DefaultLastName, DefaultMiddleName);

            // Act
            var result = _validator.TestValidate(register);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Email)
                .WithErrorCode(ValidationErrorCodes.PropertyTooLarge)
                .Only();
        }

        [Test]
        public void Email_Empty_Fails()
        {
            // Arrange
            RegisterDto register = new(string.Empty, DefaultFirstName, DefaultLastName, DefaultMiddleName);

            // Act
            var result = _validator.TestValidate(register);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Email)
                .WithErrorCode(ValidationErrorCodes.PropertyRequired)
                .Only();
        }

        [Test]
        public void Email_Invalid_Fails()
        {
            // Arrange
            RegisterDto register = new("invalid", DefaultFirstName, DefaultLastName, DefaultMiddleName);

            // Act
            var result = _validator.TestValidate(register);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Email)
                .WithErrorCode(ValidationErrorCodes.InvalidEmail)
                .Only();
        }

        [Test]
        [TestCase("Jane")]
        [TestCase("Максим")]
        [TestCase("Włodzimierz")]
        [TestCase("大麓")]
        [TestCase("حكيم")]
        public void FirstName_Valid_Succeeds(string firstName)
        {
            // Arrange
            RegisterDto register = new(DefaultEmail, firstName, DefaultLastName, DefaultMiddleName);

            // Act
            var result = _validator.TestValidate(register);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }

        [Test]
        public void FirstName_Empty_Fails()
        {
            // Arrange
            RegisterDto register = new(DefaultEmail, string.Empty, DefaultLastName, DefaultMiddleName);

            // Act
            var result = _validator.TestValidate(register);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.FirstName)
                .WithErrorCode(ValidationErrorCodes.PropertyRequired)
                .Only();
        }

        [Test]
        public void FirstName_TooLarge_Fails()
        {
            // Arrange
            string bigFirstName = new('a', ApplicationUser.MaxNameLength + 1);
            RegisterDto register = new(DefaultEmail, bigFirstName, DefaultLastName, DefaultMiddleName);

            // Act
            var result = _validator.TestValidate(register);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.FirstName)
                .WithErrorCode(ValidationErrorCodes.PropertyTooLarge)
                .Only();
        }

        [Test]
        [TestCase("O'Neill")]
        [TestCase("Скляр")]
        [TestCase("Brzęczyszczykiewicz")]
        [TestCase("鈴木")]
        [TestCase("أَحْمَدَ")]
        public void LastName_Valid_Succeeds(string lastName)
        {
            // Arrange
            RegisterDto register = new(DefaultEmail, DefaultFirstName, lastName, DefaultMiddleName);

            // Act
            var result = _validator.TestValidate(register);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }

        [Test]
        public void LastName_Empty_Fails()
        {
            // Arrange
            RegisterDto register = new(DefaultEmail, DefaultFirstName, string.Empty, DefaultMiddleName);

            // Act
            var result = _validator.TestValidate(register);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.LastName)
                .WithErrorCode(ValidationErrorCodes.PropertyRequired)
                .Only();
        }

        [Test]
        public void LastName_TooLarge_Fails()
        {
            // Arrange
            string bigLastName = new('a', ApplicationUser.MaxNameLength + 1);
            RegisterDto register = new(DefaultEmail, DefaultFirstName, bigLastName, DefaultMiddleName);

            // Act
            var result = _validator.TestValidate(register);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.LastName)
                .WithErrorCode(ValidationErrorCodes.PropertyTooLarge)
                .Only();
        }

        [Test]
        [TestCase("Orion")]
        [TestCase("Зиновій")]
        [TestCase("Michał")]
        public void MiddleName_Valid_Succeeds(string middleName)
        {
            // Arrange
            RegisterDto register = new(DefaultEmail, DefaultFirstName, DefaultLastName, middleName);

            // Act
            var result = _validator.TestValidate(register);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }

        [Test]
        [TestCase(null)]
        [TestCase("")]
        [TestCase("   ")]
        public void MiddleName_Empty_Succeeds(string middleName)
        {
            // Arrange
            RegisterDto register = new(DefaultEmail, DefaultFirstName, DefaultLastName, middleName);

            // Act
            var result = _validator.TestValidate(register);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }

        [Test]
        public void MiddleName_TooLarge_Fails()
        {
            // Arrange
            string bigMiddleName = new('a', ApplicationUser.MaxNameLength + 1);
            RegisterDto register = new(DefaultEmail, DefaultFirstName, DefaultLastName, bigMiddleName);

            // Act
            var result = _validator.TestValidate(register);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.MiddleName)
                .WithErrorCode(ValidationErrorCodes.PropertyTooLarge)
                .Only();
        }
    }
}
