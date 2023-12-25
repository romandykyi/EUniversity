using EUniversity.Core.Dtos.Users;
using EUniversity.Core.Models;
using FluentValidation;
using FluentValidation.Results;

namespace EUniversity.Core.Validation.Users;

public class EditUserDtoValidator : RegisterDtoValidator<EditUserDto>
{
    public EditUserDtoValidator() : base()
    {
        RuleFor(r => r.UserName).NotEmpty()
            .WithErrorCode(ValidationErrorCodes.PropertyRequired)
            .WithMessage("Username is required")
            .DependentRules(() =>
            {
                RuleFor(r => r.UserName).Custom((userName, context) =>
                {
                    if (userName.All(c => ApplicationUser.AllowedUserNameCharacters.Contains(c)))
                    {
                        return;
                    }
                    ValidationFailure failure = new()
                    {
                        AttemptedValue = userName,
                        ErrorCode = ValidationErrorCodes.InvalidUserName,
                        ErrorMessage = "Username is invalid",
                        PropertyName = context.PropertyPath,
                        Severity = Severity.Error
                    };
                    context.AddFailure(failure);
                });
            });
        RuleFor(x => x.UserName)
            .MaximumLength(ApplicationUser.MaxUserNameLength)
            .WithErrorCode(ValidationErrorCodes.PropertyTooLarge)
            .WithMessage($"Username cannot have more than {ApplicationUser.MaxEmailLength} characters");
    }
}
