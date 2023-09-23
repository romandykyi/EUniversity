using EUniversity.Core.Dtos.Auth;
using FluentValidation;

namespace EUniversity.Core.Validation
{
	public class LogInDtoValidator : AbstractValidator<LogInDto>
	{
		public LogInDtoValidator()
		{
			RuleFor(l => l.UserName).NotEmpty();
			RuleFor(l => l.Password).NotEmpty();
		}
	}
}
