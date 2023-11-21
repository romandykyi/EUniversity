using EUniversity.Core.Dtos.University;
using EUniversity.Core.Models.University;
using EUniversity.Core.Services;
using FluentValidation;

namespace EUniversity.Core.Validation.University;

public class CourseCreateDtoValidator : AbstractValidator<CourseCreateDto>
{
    public CourseCreateDtoValidator(IEntityExistenceChecker existenceChecker)
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithErrorCode(ValidationErrorCodes.PropertyRequired)
            .WithMessage("Course name is required");

        RuleFor(x => x.Name)
            .MaximumLength(Course.MaxNameLength)
            .WithErrorCode(ValidationErrorCodes.PropertyTooLarge)
            .WithMessage($"Course name cannot exceed {Course.MaxNameLength} characters");

        RuleFor(x => x.Description)
            .MaximumLength(Course.MaxDescriptionLength)
            .WithErrorCode(ValidationErrorCodes.PropertyTooLarge)
            .WithMessage($"Course description cannot exceed {Course.MaxNameLength} characters");
    }
}
