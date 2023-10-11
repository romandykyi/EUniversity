using EUniversity.Core.Dtos.Auth;
using FluentValidation;

namespace EUniversity.Core.Validation.Auth
{
    public class ChangePasswordDtoValidator : AbstractValidator<ChangePasswordDto>
    {
        public ChangePasswordDtoValidator()
        {
            RuleFor(x => x.Current).NotEmpty()
                .WithErrorCode(ValidationErrorCodes.PropertyRequired)
                .WithMessage("Current password is required");
            RuleFor(x => x.New).NotEmpty()
                .WithErrorCode(ValidationErrorCodes.PropertyRequired)
                .WithMessage("New password is required")
                .DependentRules(() =>
            {
                RuleFor(x => x.New).NotEqual(x => x.Current)
                    .WithErrorCode(ValidationErrorCodes.Equal)
                    .WithMessage("New password cannot be the same as old password");
            });
        }
    }
}
