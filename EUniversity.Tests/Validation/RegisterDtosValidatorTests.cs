using EUniversity.Core.Dtos.Auth;
using EUniversity.Core.Validation;
using FluentValidation.TestHelper;

namespace EUniversity.Tests.Validation
{
	public class RegisterDtosValidatorTests
	{
		private RegisterDtosValidator _validator;
		private readonly RegisterDto _validRegisterDto = 
			new("example@email.com", "Joe", "Doe");

		[OneTimeSetUp]
		public void OneTimeSetUp()
		{
			_validator = new RegisterDtosValidator();
		}

		[Test]
		public void Dtos_ValidChildren_IsValid()
		{
			// Arrange
			IEnumerable<RegisterDto> dtos = new RegisterDto[1]
			{
				_validRegisterDto
			};

			// Act
			var result = _validator.TestValidate(dtos);

			// Assert
			result.ShouldNotHaveAnyValidationErrors();
		}

		[Test]
		public void Dtos_Empty_IsInvalid()
		{
			// Arrange
			IEnumerable<RegisterDto> dtos = Array.Empty<RegisterDto>();

			// Act
			var result = _validator.TestValidate(dtos);

			// Assert
			result.ShouldHaveValidationErrorFor(x => x)
				.WithErrorCode(ValidationErrorCodes.EmptyCollection)
				.Only();
		}

		[Test]
		public void Dtos_InvalidChildElement_IsInvalid()
		{
			// Arrange
			IEnumerable<RegisterDto> dtos = new RegisterDto[2]
			{
				_validRegisterDto,
				new RegisterDto("invalid-email", string.Empty, string.Empty)
			};

			// Act
			var result = _validator.TestValidate(dtos);

			// Assert
			result.ShouldHaveAnyValidationError();
		}
	}
}
