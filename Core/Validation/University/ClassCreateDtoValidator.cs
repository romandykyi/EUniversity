using EUniversity.Core.Dtos.University;
using EUniversity.Core.Models;
using EUniversity.Core.Services;
using FluentValidation;
using Microsoft.AspNetCore.Identity;

namespace EUniversity.Core.Validation.University;

public class ClassCreateDtoValidator : ClassWriteDtoValidator<ClassCreateDto>
{
    public ClassCreateDtoValidator(IEntityExistenceChecker existenceChecker, UserManager<ApplicationUser> userManager) : base(existenceChecker, userManager)
    {
        RuleFor(c => c.Repeats)
            .GreaterThan(0)
            .When(c => c.Repeats != null)
            .WithErrorCode(ValidationErrorCodes.InvalidRange)
            .WithMessage("Repeats must be a positive number");
        RuleFor(c => c.RepeatsDelayDays)
            .GreaterThan(0)
            .When(c => c.RepeatsDelayDays != null)
            .WithErrorCode(ValidationErrorCodes.InvalidRange)
            .WithMessage("RepeatsDelayDays must be a positive number");

        RuleFor(c => c.Repeats)
            .NotNull()
            .When(c => c.RepeatsDelayDays != null)
            .WithErrorCode(ValidationErrorCodes.PropertyRequired)
            .WithMessage("Repeats must be specified when using RepeatsDelayDays");
        RuleFor(c => c.RepeatsDelayDays)
            .NotNull()
            .When(c => c.Repeats != null)
            .WithErrorCode(ValidationErrorCodes.PropertyRequired)
            .WithMessage("RepeatsDelayDays must be specified when using Repeats");
    }
}
