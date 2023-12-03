using EUniversity.Core.Dtos.University;
using EUniversity.Core.Models;
using EUniversity.Core.Models.University;
using EUniversity.Core.Policy;
using EUniversity.Core.Services;
using EUniversity.Core.Validation.Extensions;
using FluentValidation;
using Microsoft.AspNetCore.Identity;

namespace EUniversity.Core.Validation.University;

public abstract class ClassWriteDtoValidator<T> : AbstractValidator<T>
    where T : IClassWriteDto
{
    public ClassWriteDtoValidator(IEntityExistenceChecker existenceChecker,
        UserManager<ApplicationUser> userManager)
    {
        RuleFor(c => c.ClassroomId)
            .MustAsync(async (id, _) =>
                await existenceChecker.ExistsAsync<Classroom, int>(id))
            .WithErrorCode(ValidationErrorCodes.InvalidForeignKey)
            .WithMessage("Classroom does not exist");
        RuleFor(c => c.GroupId)
            .MustAsync(async (id, _) =>
                await existenceChecker.ExistsAsync<Group, int>(id))
            .WithErrorCode(ValidationErrorCodes.InvalidForeignKey)
            .WithMessage("Group does not exist");

        RuleFor(c => c.SubstituteTeacherId!)
            .IsIdOfValidUserInRole(userManager, Roles.Teacher)
            .When(c => !string.IsNullOrWhiteSpace(c.SubstituteTeacherId));

        RuleFor(c => c.DurationTicks)
            .GreaterThan(0)
            .WithErrorCode(ValidationErrorCodes.InvalidRange)
            .WithMessage("DurationTicks must be a positive number");
    }
}
