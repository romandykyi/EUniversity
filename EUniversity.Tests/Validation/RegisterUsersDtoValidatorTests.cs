using EUniversity.Core.Dtos.Auth;
using EUniversity.Core.Dtos.Users;
using EUniversity.Core.Validation;
using FluentValidation.TestHelper;

namespace EUniversity.Tests.Validation
{
	public class RegisterUsersDtoValidatorTests
	{
		private RegisterUsersDtoValidator _validator;
		private readonly RegisterDto _validRegisterDto =
			new() { Email = "example@email.com", FirstName = "Joe", LastName = "Doe" };

		[OneTimeSetUp]
		public void OneTimeSetUp()
		{
			_validator = new();
		}

		[Test]
		public void Dtos_ValidChildren_IsValid()
		{
			// Arrange
			RegisterUsersDto dto = new()
			{
				Users = new RegisterDto[1] { _validRegisterDto }
			};

			// Act
			var result = _validator.TestValidate(dto);

			// Assert
			result.ShouldNotHaveAnyValidationErrors();
		}

		[Test]
		public void Dtos_Empty_IsInvalid()
		{
			// Arrange
			RegisterUsersDto dto = new()
			{
				Users = Enumerable.Empty<RegisterDto>()
			};

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
			RegisterUsersDto dto = new()
			{
				Users = new RegisterDto[2]
				{
					_validRegisterDto,
					new RegisterDto()
				}
			};

			// Act
			var result = _validator.TestValidate(dto);

			// Assert
			result.ShouldHaveAnyValidationError();
		}
	}
}
