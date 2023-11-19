using EUniversity.Core.Dtos.University;
using EUniversity.Core.Models;
using EUniversity.Core.Policy;
using EUniversity.Core.Validation.Extensions;
using FluentValidation;
using Microsoft.AspNetCore.Identity;

namespace EUniversity.Core.Validation.University;

public class AssignStudentDtoValidator : AbstractValidator<AssignStudentDto>
{
    public AssignStudentDtoValidator(UserManager<ApplicationUser> userManager)
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
    }
}
