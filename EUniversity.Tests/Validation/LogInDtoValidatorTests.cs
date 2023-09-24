using EUniversity.Core.Dtos.Auth;
using EUniversity.Core.Validation;

namespace EUniversity.Tests.Validator
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
			var result = _validator.Validate(logInDto);

			// Assert
			Assert.That(result.IsValid);
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
			var result = _validator.Validate(logInDto);

			// Assert
			Assert.Multiple(() =>
			{
				Assert.That(result.IsValid, Is.Not.True);
				Assert.That(result.Errors[0].ErrorMessage, Is.EqualTo("Username is required"));
			});
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
			var result = _validator.Validate(logInDto);

			// Assert
			Assert.Multiple(() =>
			{
				Assert.That(result.IsValid, Is.Not.True);
				Assert.That(result.Errors[0].ErrorMessage, Is.EqualTo("Password is required"));
			});
		}
	}
}
