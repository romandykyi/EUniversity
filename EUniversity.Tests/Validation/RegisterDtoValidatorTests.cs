using EUniversity.Core.Dtos.Auth;
using EUniversity.Core.Models;
using EUniversity.Core.Validation;
using FluentValidation.TestHelper;

namespace EUniversity.Tests.Validation
{
	public class RegisterDtoValidatorTests
	{
		private RegisterDtoValidator _validator;

		private RegisterDto register;

		[SetUp]
		public void SetUp()
		{
			// Valid register for all properties
			register = new()
			{
				UserName = "user",
				Email = "email@email.com",
				Password = "Passw0rd123",
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
			register.Email = email;

			// Act
			var result = _validator.TestValidate(register);

			// Assert
			result.ShouldNotHaveAnyValidationErrors();
		}

		[Test]
		public void Email_TooLarge_IsInvalid()
		{
			// Arrange
			string bigString = new('a', ApplicationUser.MaxEmailLength);
			register.Email = $"{bigString}@email.com";

			// Act
			var result = _validator.TestValidate(register);

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
			register.Email = email;

			// Act
			var result = _validator.TestValidate(register);

			// Assert
			result.ShouldHaveValidationErrorFor(x => x.Email)
				.WithErrorMessage("Valid email is required")
				.Only();
		}

		[Test]
		[TestCase("username")]
		[TestCase("User_Name")]
		[TestCase("user-name")]
		[TestCase("user.name")]
		[TestCase("userName123")]
		public void UserName_Valid_IsValid(string userName)
		{
			// Arrange
			register.UserName = userName;

			// Act
			var result = _validator.TestValidate(register);

			// Assert
			result.ShouldNotHaveAnyValidationErrors();
		}

		[Test]
		public void UserName_Empty_IsInvalid()
		{
			// Arrange
			register.UserName = string.Empty;

			// Act
			var result = _validator.TestValidate(register);

			// Assert
			result.ShouldHaveValidationErrorFor(x => x.UserName)
				.WithErrorMessage("Username is required")
				.Only();
		}

		[Test]
		public void UserName_TooLarge_IsInvalid()
		{
			// Arrange
			register.UserName = new('a', ApplicationUser.MaxUserNameLength + 1);

			// Act
			var result = _validator.TestValidate(register);

			// Assert
			result.ShouldHaveValidationErrorFor(x => x.UserName)
				.WithErrorMessage($"Username cannot have more than {ApplicationUser.MaxUserNameLength} characters")
				.Only();
		}

		[Test]
		[TestCase("user name")]
		[TestCase("/!, -")]
		[TestCase("user1?")]
		[TestCase("user300$")]
		public void UserName_Invalid_IsInvalid(string userName)
		{
			// Arrange
			register.UserName = userName;

			// Act
			var result = _validator.TestValidate(register);

			// Assert
			result.ShouldHaveValidationErrorFor(x => x.UserName)
				.WithErrorMessage("Username contains invalid characters")
				.Only();
		}

		[Test]
		[TestCase("Password1")]
		[TestCase("pAss300$")]
		[TestCase("wp8rXW9U5u9x")]
		public void Password_Valid_IsValid(string password)
		{
			// Arrange
			register.Password = password;

			// Act
			var result = _validator.TestValidate(register);

			// Assert
			result.ShouldNotHaveAnyValidationErrors();
		}

		[Test]
		public void Password_TooLarge_IsInvalid()
		{
			// Arrange
			string bigString = new('*', ApplicationUser.MaxPasswordLength);
			register.Password = $"Ab1{bigString}";

			// Act
			var result = _validator.TestValidate(register);

			// Assert
			result.ShouldHaveValidationErrorFor(x => x.Password)
				.WithErrorMessage($"Password cannot have more than {ApplicationUser.MaxPasswordLength} characters")
				.Only();
		}

		[Test]
		[TestCase("")]
		[TestCase("12345")]
		[TestCase("password")]
		[TestCase("aCb12j")]
		public void Password_Weak_IsInvalid(string password)
		{
			// Arrange
			register.Password = password;

			// Act
			var result = _validator.TestValidate(register);

			// Assert
			result.ShouldHaveValidationErrorFor(x => x.Password)
				.WithErrorMessage("Password must be at least 8 characters long and contain at least one uppercase letter, one lowercase letter, and one number")
				.Only();
		}
	}
}
