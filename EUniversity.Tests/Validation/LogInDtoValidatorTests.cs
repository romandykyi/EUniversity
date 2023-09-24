using EUniversity.Core.Dtos.Auth;
using EUniversity.Core.Validation;
using FluentValidation.TestHelper;

namespace EUniversity.Tests.Validation
{
	public class LogInDtoValidatorTests
	{
		private LogInDtoValidator _validator;

		[OneTimeSetUp]
		public void SetUp()
		{
			_validator = new LogInDtoValidator();
		}

		[Test]
		public void ValidInput_IsValid()
		{
			// Arrange
			var logInDto = new LogInDto
			{
				UserName = "testuser",
				Password = "password123"
			};

			// Act
			var result = _validator.TestValidate(logInDto);

			// Assert
			result.ShouldNotHaveAnyValidationErrors();
		}

		[Test]
		public void EmptyUserName_IsInvalid()
		{
			// Arrange
			var logInDto = new LogInDto
			{
				UserName = "",
				Password = "password123"
			};

			// Act
			var result = _validator.TestValidate(logInDto);

			// Assert
			result.ShouldHaveValidationErrorFor(x => x.UserName)
				.WithErrorMessage("Username is required")
				.Only();
		}

		[Test]
		public void EmptyPassword_IsInvalid()
		{
			// Arrange
			var logInDto = new LogInDto
			{
				UserName = "testuser",
				Password = ""
			};

			// Act
			var result = _validator.TestValidate(logInDto);

			// Assert
			result.ShouldHaveValidationErrorFor(x => x.Password)
				.WithErrorMessage("Password is required")
				.Only();
		}
	}
}
