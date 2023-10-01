using EUniversity.Core.Dtos.Auth;
using EUniversity.Core.Models;
using FluentValidation;

namespace EUniversity.Core.Validation
{
	public class RegisterDtoValidator : AbstractValidator<RegisterDto>
	{
		public RegisterDtoValidator()
		{
			// Email
			RuleFor(r => r.Email).MaximumLength(ApplicationUser.MaxEmailLength)
				.WithErrorCode(ValidationErrorCodes.PropertyTooLarge)
				.WithMessage($"Email cannot have more than {ApplicationUser.MaxEmailLength} characters");
			RuleFor(r => r.Email).NotEmpty()
				.WithErrorCode(ValidationErrorCodes.PropertyRequired)
				.WithMessage("Email is required")
				.DependentRules(() =>
				{
					RuleFor(r => r.Email).EmailAddress()
						.WithErrorCode(ValidationErrorCodes.InvalidEmail)
						.WithMessage("Email is invalid");
				});

			// First name
			RuleFor(r => r.FirstName).NotEmpty()
				.WithErrorCode(ValidationErrorCodes.PropertyRequired)
				.WithMessage("First name is required");
			RuleFor(r => r.FirstName).MaximumLength(ApplicationUser.MaxNameLength)
				.WithErrorCode(ValidationErrorCodes.PropertyTooLarge)
				.WithMessage($"First name cannot have more than {ApplicationUser.MaxNameLength} characters");

			// Last name
			RuleFor(r => r.LastName).NotEmpty()
				.WithErrorCode(ValidationErrorCodes.PropertyRequired)
				.WithMessage("Last name is required");
			RuleFor(r => r.LastName).MaximumLength(ApplicationUser.MaxNameLength)
				.WithErrorCode(ValidationErrorCodes.PropertyTooLarge)
				.WithMessage($"Last name cannot have more than {ApplicationUser.MaxNameLength} characters");

			// Middle name
			RuleFor(r => r.MiddleName).MaximumLength(ApplicationUser.MaxNameLength)
				.WithErrorCode(ValidationErrorCodes.PropertyTooLarge)
				.WithMessage($"Middle name cannot have more than {ApplicationUser.MaxNameLength} characters");
		}
	}
}
