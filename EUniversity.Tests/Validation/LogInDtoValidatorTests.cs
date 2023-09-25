using EUniversity.Core.Dtos.Auth;
using EUniversity.Core.Validation;
using FluentValidation.TestHelper;

namespace EUniversity.Tests.Validation
{
	public class LogInDtoValidatorTests
	{
		private LogInDtoValidator _validator;
		private LogInDto _login;

		[SetUp]
		public void SetUp()
		{
			// Valid login for all properties
			_login = new()
			{
				UserName = "user",
				Password = "Passw0rd123"
			};
		}

		[OneTimeSetUp]
		public void OneTimeSetUp()
		{
			_validator = new LogInDtoValidator();
		}

		[Test]
		public void Input_Valid_IsValid()
		{
			// Arrange
			var login = _login;

			// Act
			var result = _validator.TestValidate(login);

			// Assert
			result.ShouldNotHaveAnyValidationErrors();
		}

		[Test]
		public void UserName_Empty_IsInvalid()
		{
			// Arrange
			_login.UserName = string.Empty;

			// Act
			var result = _validator.TestValidate(_login);

			// Assert
			result.ShouldHaveValidationErrorFor(x => x.UserName)
				.WithErrorMessage("Username is required")
				.Only();
		}

		[Test]
		public void Password_Empty_IsInvalid()
		{
			// Arrange
			_login.Password = string.Empty;

			// Act
			var result = _validator.TestValidate(_login);

			// Assert
			result.ShouldHaveValidationErrorFor(x => x.Password)
				.WithErrorMessage("Password is required")
				.Only();
		}
	}
}
