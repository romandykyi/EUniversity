using EUniversity.Core.Dtos.University;
using EUniversity.Core.Models.University;
using FluentValidation;

namespace EUniversity.Core.Validation.University;

public class ClassTypeCreateDtoValidator : AbstractValidator<ClassTypeCreateDto>
{
    public ClassTypeCreateDtoValidator()
    {
        RuleFor(c => c.Name)
            .NotEmpty()
            .WithErrorCode(ValidationErrorCodes.PropertyRequired)
            .WithMessage("Class type name is required");

        RuleFor(c => c.Name)
            .MaximumLength(ClassType.MaxNameLength)
            .WithErrorCode(ValidationErrorCodes.PropertyTooLarge)
            .WithMessage($"Class type name cannot exceed {ClassType.MaxNameLength} characters");
    }
}
