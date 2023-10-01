using EUniversity.Core.Dtos.Auth;
using EUniversity.Core.Validation;
using FluentValidation.TestHelper;

namespace EUniversity.Tests.Validation
{
	public class ChangePasswordDtoValidatorTests
	{
		private ChangePasswordDtoValidator _validator;
		private ChangePasswordDto _password;

		[SetUp]
		public void SetUp()
		{
			// Valid input for all properties
			_password = new()
			{
				Current = "password",
				New = "Password1!",
			};
		}

		[OneTimeSetUp]
		public void OneTimeSetUp()
		{
			_validator = new ChangePasswordDtoValidator();
		}

		[Test]
		public void Input_Valid_Succeeds()
		{
			// Arrange
			var password = _password;

			// Act
			var result = _validator.TestValidate(password);

			// Assert
			result.ShouldNotHaveAnyValidationErrors();
		}

		[Test]
		public void OldPassword_Empty_Fails()
		{
			// Arrange
			_password.Current = string.Empty;

			// Act
			var result = _validator.TestValidate(_password);

			// Assert
			result.ShouldHaveValidationErrorFor(x => x.Current)
				.WithErrorCode(ValidationErrorCodes.PropertyRequired)
				.Only();
		}

		[Test]
		public void NewPassword_Empty_Succeeds()
		{
			// Arrange
			_password.New = string.Empty;

			// Act
			var result = _validator.TestValidate(_password);

			// Assert
			result.ShouldHaveValidationErrorFor(x => x.New)
				.WithErrorCode(ValidationErrorCodes.PropertyRequired)
				.Only();
		}

		[Test]
		public void Passwords_Empty_FailsWithoutEqualErrorCode()
		{
			// Arrange
			_password.Current = _password.New = string.Empty;

			// Act
			var result = _validator.TestValidate(_password);

			// Assert
			result.ShouldHaveValidationErrorFor(x => x.New)
				.WithoutErrorCode(ValidationErrorCodes.Equal);
		}

		[Test]
		public void Passwords_Equal_Fails()
		{
			// Arrange
			_password.Current = _password.New;

			// Act
			var result = _validator.TestValidate(_password);

			// Assert
			result.ShouldHaveValidationErrorFor(x => x.New)
				.WithErrorCode(ValidationErrorCodes.Equal)
				.Only();
		}
	}
}
