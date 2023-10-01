using EUniversity.Core.Dtos.Auth;
using FluentValidation;

namespace EUniversity.Core.Validation
{
	public class LogInDtoValidator : AbstractValidator<LogInDto>
	{
		public LogInDtoValidator()
		{
			RuleFor(l => l.UserName).NotEmpty()
				.WithErrorCode(ValidationErrorCodes.PropertyRequired)
				.WithMessage("Username is required");
			RuleFor(l => l.Password).NotEmpty()
				.WithErrorCode(ValidationErrorCodes.PropertyRequired)
				.WithMessage("Password is required");
		}
	}
}
