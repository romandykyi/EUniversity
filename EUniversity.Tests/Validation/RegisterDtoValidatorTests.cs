using EUniversity.Core.Dtos.Auth;
using EUniversity.Core.Models;
using EUniversity.Core.Validation;
using FluentValidation.TestHelper;

namespace EUniversity.Tests.Validation
{
	public class RegisterDtoValidatorTests
	{
		private RegisterDtoValidator _validator;
		private RegisterDto _register;

		[SetUp]
		public void SetUp()
		{
			// Valid register for all properties
			_register = new()
			{
				Email = "heisenberg@blue-crystals.com",
				FirstName = "Walter",
				MiddleName = "Hartwell",
				LastName = "White"
			};
		}

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
			_register.Email = email;

			// Act
			var result = _validator.TestValidate(_register);

			// Assert
			result.ShouldNotHaveAnyValidationErrors();
		}

		[Test]
		public void Email_TooLarge_Fails()
		{
			// Arrange
			string bigString = new('a', ApplicationUser.MaxEmailLength);
			_register.Email = $"{bigString}@email.com";

			// Act
			var result = _validator.TestValidate(_register);

			// Assert
			result.ShouldHaveValidationErrorFor(x => x.Email)
				.WithErrorCode(ValidationErrorCodes.PropertyTooLarge)
				.Only();
		}

		[Test]
		public void Email_Empty_Fails()
		{
			// Arrange
			_register.Email = string.Empty;

			// Act
			var result = _validator.TestValidate(_register);

			// Assert
			result.ShouldHaveValidationErrorFor(x => x.Email)
				.WithErrorCode(ValidationErrorCodes.PropertyRequired)
				.Only();
		}

		[Test]
		public void Email_Invalid_Fails()
		{
			// Arrange
			_register.Email = "invalid";

			// Act
			var result = _validator.TestValidate(_register);

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
			_register.FirstName = firstName;

			// Act
			var result = _validator.TestValidate(_register);

			// Assert
			result.ShouldNotHaveAnyValidationErrors();
		}

		[Test]
		public void FirstName_Empty_Fails()
		{
			// Arrange
			_register.FirstName = string.Empty;

			// Act
			var result = _validator.TestValidate(_register);

			// Assert
			result.ShouldHaveValidationErrorFor(x => x.FirstName)
				.WithErrorCode(ValidationErrorCodes.PropertyRequired)
				.Only();
		}

		[Test]
		public void FirstName_TooLarge_Fails()
		{
			// Arrange
			_register.FirstName = new('a', ApplicationUser.MaxNameLength + 1);

			// Act
			var result = _validator.TestValidate(_register);

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
			_register.LastName = lastName;

			// Act
			var result = _validator.TestValidate(_register);

			// Assert
			result.ShouldNotHaveAnyValidationErrors();
		}

		[Test]
		public void LastName_Empty_Fails()
		{
			// Arrange
			_register.LastName = string.Empty;

			// Act
			var result = _validator.TestValidate(_register);

			// Assert
			result.ShouldHaveValidationErrorFor(x => x.LastName)
				.WithErrorCode(ValidationErrorCodes.PropertyRequired)
				.Only();
		}

		[Test]
		public void LastName_TooLarge_Fails()
		{
			// Arrange
			_register.LastName = new('a', ApplicationUser.MaxNameLength + 1);

			// Act
			var result = _validator.TestValidate(_register);

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
			_register.MiddleName = middleName;

			// Act
			var result = _validator.TestValidate(_register);

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
			_register.MiddleName = middleName;

			// Act
			var result = _validator.TestValidate(_register);

			// Assert
			result.ShouldNotHaveAnyValidationErrors();
		}

		[Test]
		public void MiddleName_TooLarge_Fails()
		{
			// Arrange
			_register.MiddleName = new('a', ApplicationUser.MaxNameLength + 1);

			// Act
			var result = _validator.TestValidate(_register);

			// Assert
			result.ShouldHaveValidationErrorFor(x => x.MiddleName)
				.WithErrorCode(ValidationErrorCodes.PropertyTooLarge)
				.Only();
		}
	}
}
