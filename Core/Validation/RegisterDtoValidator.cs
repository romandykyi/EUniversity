using EUniversity.Core.Dtos.Auth;
using EUniversity.Core.Models;
using FluentValidation;

namespace EUniversity.Core.Validation
{
	public class RegisterDtoValidator : AbstractValidator<RegisterDto>
	{
		public RegisterDtoValidator()
		{
			RuleFor(r => r.Email).MaximumLength(ApplicationUser.MaxEmailLength)
				.WithMessage($"Email cannot have more than {ApplicationUser.MaxEmailLength} characters");
			RuleFor(r => r.Email).EmailAddress().WithMessage("Valid email is required");

			RuleFor(r => r.FirstName).NotEmpty().WithMessage("First name is required");
			RuleFor(r => r.FirstName).MaximumLength(ApplicationUser.MaxNameLength)
				.WithMessage($"First name cannot have more than {ApplicationUser.MaxNameLength} characters");
			RuleFor(r => r.LastName).NotEmpty().WithMessage("Last name is required");
			RuleFor(r => r.LastName).MaximumLength(ApplicationUser.MaxNameLength)
				.WithMessage($"Last name cannot have more than {ApplicationUser.MaxNameLength} characters");

			RuleFor(r => r.MiddleName).MaximumLength(ApplicationUser.MaxNameLength)
				.WithMessage($"Middle name cannot have more than {ApplicationUser.MaxNameLength} characters");
		}
	}
}
