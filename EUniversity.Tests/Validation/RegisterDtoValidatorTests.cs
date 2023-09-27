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
		public void Email_Valid_IsValid(string email)
		{
			// Arrange
			_register.Email = email;

			// Act
			var result = _validator.TestValidate(_register);

			// Assert
			result.ShouldNotHaveAnyValidationErrors();
		}

		[Test]
		public void Email_TooLarge_IsInvalid()
		{
			// Arrange
			string bigString = new('a', ApplicationUser.MaxEmailLength);
			_register.Email = $"{bigString}@email.com";

			// Act
			var result = _validator.TestValidate(_register);

			// Assert
			result.ShouldHaveValidationErrorFor(x => x.Email)
				.WithErrorMessage($"Email cannot have more than {ApplicationUser.MaxEmailLength} characters")
				.Only();
		}

		[Test]
		[TestCase("")]
		[TestCase("invalid")]
		public void Email_Invalid_IsInvalid(string email)
		{
			// Arrange
			_register.Email = email;

			// Act
			var result = _validator.TestValidate(_register);

			// Assert
			result.ShouldHaveValidationErrorFor(x => x.Email)
				.WithErrorMessage("Valid email is required")
				.Only();
		}

		[Test]
		[TestCase("Jane")]
		[TestCase("Максим")]
		[TestCase("Włodzimierz")]
		[TestCase("大麓")]
		[TestCase("حكيم")]
		public void FirstName_Valid_IsValid(string firstName)
		{
			// Arrange
			_register.FirstName = firstName;

			// Act
			var result = _validator.TestValidate(_register);

			// Assert
			result.ShouldNotHaveAnyValidationErrors();
		}

		[Test]
		public void FirstName_Empty_IsInvalid()
		{
			// Arrange
			_register.FirstName = string.Empty;

			// Act
			var result = _validator.TestValidate(_register);

			// Assert
			result.ShouldHaveValidationErrorFor(x => x.FirstName)
				.WithErrorMessage($"First name is required")
				.Only();
		}

		[Test]
		public void FirstName_TooLarge_IsInvalid()
		{
			// Arrange
			_register.FirstName = new('a', ApplicationUser.MaxNameLength + 1);

			// Act
			var result = _validator.TestValidate(_register);

			// Assert
			result.ShouldHaveValidationErrorFor(x => x.FirstName)
				.WithErrorMessage($"First name cannot have more than {ApplicationUser.MaxNameLength} characters")
				.Only();
		}

		[Test]
		[TestCase("O'Neill")]
		[TestCase("Скляр")]
		[TestCase("Brzęczyszczykiewicz")]
		[TestCase("鈴木")]
		[TestCase("أَحْمَدَ")]
		public void LastName_Valid_IsValid(string lastName)
		{
			// Arrange
			_register.LastName = lastName;

			// Act
			var result = _validator.TestValidate(_register);

			// Assert
			result.ShouldNotHaveAnyValidationErrors();
		}

		[Test]
		public void LastName_Empty_IsInvalid()
		{
			// Arrange
			_register.LastName = string.Empty;

			// Act
			var result = _validator.TestValidate(_register);

			// Assert
			result.ShouldHaveValidationErrorFor(x => x.LastName)
				.WithErrorMessage($"Last name is required")
				.Only();
		}

		[Test]
		public void LastName_TooLarge_IsInvalid()
		{
			// Arrange
			_register.LastName = new('a', ApplicationUser.MaxNameLength + 1);

			// Act
			var result = _validator.TestValidate(_register);

			// Assert
			result.ShouldHaveValidationErrorFor(x => x.LastName)
				.WithErrorMessage($"Last name cannot have more than {ApplicationUser.MaxNameLength} characters")
				.Only();
		}

		[Test]
		[TestCase("Orion")]
		[TestCase("Зиновій")]
		[TestCase("Michał")]
		public void MiddleName_Valid_IsValid(string middleName)
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
		public void MiddleName_Empty_IsValid(string middleName)
		{
			// Arrange
			_register.MiddleName = middleName;

			// Act
			var result = _validator.TestValidate(_register);

			// Assert
			result.ShouldNotHaveAnyValidationErrors();
		}

		[Test]
		public void MiddleName_TooLarge_IsInvalid()
		{
			// Arrange
			_register.MiddleName = new('a', ApplicationUser.MaxNameLength + 1);

			// Act
			var result = _validator.TestValidate(_register);

			// Assert
			result.ShouldHaveValidationErrorFor(x => x.MiddleName)
				.WithErrorMessage($"Middle name cannot have more than {ApplicationUser.MaxNameLength} characters")
				.Only();
		}
	}
}
