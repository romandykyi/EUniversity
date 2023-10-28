using EUniversity.Core.Dtos.University;
using EUniversity.Core.Models;
using EUniversity.Core.Models.University;
using EUniversity.Core.Policy;
using EUniversity.Core.Services;
using EUniversity.Core.Validation.Extensions;
using FluentValidation;
using Microsoft.AspNetCore.Identity;

namespace EUniversity.Core.Validation.University;

public class GroupCreateDtoValidator : AbstractValidator<GroupCreateDto>
{
    public GroupCreateDtoValidator(IEntityExistenceChecker existenceChecker,
        UserManager<ApplicationUser> userManager)
    {
        RuleFor(g => g.Name)
            .NotEmpty()
            .WithErrorCode(ValidationErrorCodes.PropertyRequired)
            .WithMessage("Group name is required");
        RuleFor(g => g.Name)
            .MaximumLength(Group.MaxNameLength)
            .WithErrorCode(ValidationErrorCodes.PropertyTooLarge)
            .WithMessage($"Group name cannot exceed {Group.MaxNameLength} characters");

        RuleFor(g => g.CourseId)
            .MustAsync(async (id, _) =>
                await existenceChecker.ExistsAsync<Course, int>(id))
            .WithErrorCode(ValidationErrorCodes.InvalidForeignKey)
            .WithMessage("Course does not exist");

        RuleFor(g => g.TeacherId!)
            .IsIdOfValidUserInRole(userManager, Roles.Teacher)
            .When(g => !string.IsNullOrWhiteSpace(g.TeacherId));
    }
}
