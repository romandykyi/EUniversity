using EUniversity.Core.Dtos.University;
using EUniversity.Core.Models;
using EUniversity.Core.Models.University;
using EUniversity.Core.Policy;
using EUniversity.Core.Services;
using EUniversity.Core.Validation.Extensions;
using FluentValidation;
using Microsoft.AspNetCore.Identity;

namespace EUniversity.Core.Validation.University;

public class StudentGroupDtoValidator : AbstractValidator<StudentGroupDto>
{
    public StudentGroupDtoValidator(IEntityExistenceChecker existenceChecker,
        UserManager<ApplicationUser> userManager)
    {
        RuleFor(sg => sg.StudentId)
            .NotEmpty()
            .WithErrorCode(ValidationErrorCodes.PropertyRequired)
            .WithMessage("Student ID is required")
            .DependentRules(() =>
            {
                RuleFor(sg => sg.StudentId)
                    .IsIdOfValidUserInRole(userManager, Roles.Student);
            });

        RuleFor(g => g.GroupId)
            .MustAsync(async (id, _) =>
                await existenceChecker.ExistsAsync<Group, int>(id))
            .WithErrorCode(ValidationErrorCodes.InvalidForeignKey)
            .WithMessage("Group does not exist");
    }
}
