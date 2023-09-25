using EUniversity.Core.Dtos.Auth;
using EUniversity.Core.Models;
using FluentValidation;

namespace EUniversity.Core.Validation
{
	public class RegisterDtoValidator : AbstractValidator<RegisterDto>
	{
		private static bool IsPasswordStrong(string s)
		{
			return s.Length >= 8 &&
				s.Any(char.IsLower) && s.Any(char.IsUpper) && s.Any(char.IsDigit);
		}

		public RegisterDtoValidator()
		{
			RuleFor(r => r.Email).MaximumLength(ApplicationUser.MaxEmailLength)
				.WithMessage($"Email cannot have more than {ApplicationUser.MaxEmailLength} characters");
			RuleFor(r => r.Email).EmailAddress().WithMessage("Valid email is required");

			RuleFor(r => r.UserName).NotEmpty().WithMessage("Username is required");
			RuleFor(r => r.UserName).MaximumLength(ApplicationUser.MaxUserNameLength)
				.WithMessage($"Username cannot have more than {ApplicationUser.MaxUserNameLength} characters");
			RuleFor(r => r.UserName).Matches(@"^[a-zA-Z0-9_.\-]*$")
				.WithMessage("Username contains invalid characters");

			RuleFor(r => r.Password).MaximumLength(ApplicationUser.MaxPasswordLength)
				.WithMessage($"Password cannot have more than {ApplicationUser.MaxPasswordLength} characters");
			RuleFor(r => r.Password).Must(IsPasswordStrong)
				.WithMessage("Password must be at least 8 characters long and contain at least one uppercase letter, one lowercase letter, and one number");
		}
	}
}
