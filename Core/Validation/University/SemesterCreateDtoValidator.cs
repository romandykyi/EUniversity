using EUniversity.Core.Dtos.University;
using EUniversity.Core.Models.University;
using FluentValidation;

namespace EUniversity.Core.Validation.University;

public class SemesterCreateDtoValidator : AbstractValidator<SemesterCreateDto>
{
    public SemesterCreateDtoValidator()
    {
        RuleFor(s => s.Name)
            .NotEmpty()
            .WithErrorCode(ValidationErrorCodes.PropertyRequired)
            .WithMessage("Semester name is required");
        RuleFor(s => s.Name)
            .MaximumLength(Semester.MaxNameLength)
            .WithErrorCode(ValidationErrorCodes.PropertyTooLarge)
            .WithMessage($"Semester name cannot exceed {Semester.MaxNameLength} characters");

        RuleFor(s => s.DateFrom)
            .LessThan(s => s.DateTo)
            .WithErrorCode(ValidationErrorCodes.InvalidRange)
            .WithMessage("The from date must be earlier than the to date");
        RuleFor(s => s.DateTo)
            .GreaterThan(DateTimeOffset.Now)
            .WithErrorCode(ValidationErrorCodes.InvalidRange)
            .WithMessage("The to date must be in the future");
    }
}
