using EUniversity.Core.Dtos.University.Grades;
using EUniversity.Core.Models.University.Grades;
using FluentValidation;

namespace EUniversity.Core.Validation.University.Grades;

public class GradeCreateDtoValidator : AbstractValidator<GradeCreateDto>
{
    public GradeCreateDtoValidator()
    {
        RuleFor(c => c.Name)
            .NotEmpty()
            .WithErrorCode(ValidationErrorCodes.PropertyRequired)
            .WithMessage("Grade name is required");

        RuleFor(c => c.Name)
            .MaximumLength(Grade.MaxNameLength)
            .WithErrorCode(ValidationErrorCodes.PropertyTooLarge)
            .WithMessage($"Grade name cannot exceed {Grade.MaxNameLength} characters");
    }
}
