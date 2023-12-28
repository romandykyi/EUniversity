using EUniversity.Core.Dtos.University;
using EUniversity.Core.Models.University;
using FluentValidation;

namespace EUniversity.Core.Validation.University;

public class ActivityTypeCreateDtoValidator : AbstractValidator<ActivityTypeCreateDto>
{
    public ActivityTypeCreateDtoValidator()
    {
        RuleFor(c => c.Name)
            .NotEmpty()
            .WithErrorCode(ValidationErrorCodes.PropertyRequired)
            .WithMessage("Activity type name is required");

        RuleFor(c => c.Name)
            .MaximumLength(ActivityType.MaxNameLength)
            .WithErrorCode(ValidationErrorCodes.PropertyTooLarge)
            .WithMessage($"Activity type name cannot exceed {ActivityType.MaxNameLength} characters");
    }
}
