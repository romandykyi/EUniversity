using EUniversity.Core.Dtos.Users;
using FluentValidation;

namespace EUniversity.Core.Validation
{
    public class RegisterUsersDtoValidator : AbstractValidator<RegisterUsersDto>
    {
        public RegisterUsersDtoValidator()
        {
            RuleFor(x => x.Users).NotEmpty()
                .WithErrorCode(ValidationErrorCodes.EmptyCollection)
                .WithMessage("Users cannot be empty");
            RuleForEach(x => x.Users)
                .SetValidator(new RegisterDtoValidator());
        }
    }
}
