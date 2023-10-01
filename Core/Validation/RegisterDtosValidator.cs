using FluentValidation;
using EUniversity.Core.Dtos.Auth;

namespace EUniversity.Core.Validation
{
	public class RegisterDtosValidator : AbstractValidator<IEnumerable<RegisterDto>>
	{
		public RegisterDtosValidator() 
		{
			RuleFor(x => x).NotEmpty()
				.WithErrorCode(ValidationErrorCodes.EmptyCollection)
				.WithMessage("Users cannot be empty");
			RuleForEach(x => x)
				.SetValidator(new RegisterDtoValidator());
		}
	}
}
