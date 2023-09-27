using FluentValidation;
using EUniversity.Core.Dtos.Auth;

namespace EUniversity.Core.Validation
{
	public class ChangePasswordDtoValidator : AbstractValidator<ChangePasswordDto>
	{
		public ChangePasswordDtoValidator() 
		{
			RuleFor(x => x.Current).NotEmpty().WithMessage("Current password is required");
			RuleFor(x => x.New).NotEmpty().WithMessage("New password is required")
				.DependentRules(() =>
			{
				RuleFor(x => x.New).NotEqual(x => x.Current)
				.WithMessage("New password cannot be the same as old password");
			});
		}
	}
}
