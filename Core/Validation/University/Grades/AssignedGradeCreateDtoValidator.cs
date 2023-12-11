using EUniversity.Core.Dtos.University.Grades;
using EUniversity.Core.Models;
using EUniversity.Core.Models.University;
using EUniversity.Core.Models.University.Grades;
using EUniversity.Core.Policy;
using EUniversity.Core.Services;
using EUniversity.Core.Validation.Extensions;
using FluentValidation;
using Microsoft.AspNetCore.Identity;

namespace EUniversity.Core.Validation.University.Grades;

public class AssignedGradeCreateDtoValidator : AbstractValidator<AssignedGradeCreateDto>
{
    public AssignedGradeCreateDtoValidator(IEntityExistenceChecker existenceChecker,
        UserManager<ApplicationUser> userManager)
    {
        RuleFor(g => g.Notes)
            .MaximumLength(AssignedGrade.MaxNotesLength)
            .WithErrorCode(ValidationErrorCodes.PropertyTooLarge)
            .WithMessage($"Notes cannot exceed {AssignedGrade.MaxNotesLength} characters");

        RuleFor(g => g.GradeId)
            .MustAsync(async (id, _) =>
                await existenceChecker.ExistsAsync<Grade, int>(id))
            .WithErrorCode(ValidationErrorCodes.InvalidForeignKey)
            .WithMessage("Grade does not exist");

        RuleFor(g => g.GroupId)
            .MustAsync(async (id, _) =>
                await existenceChecker.ExistsAsync<Group, int>(id))
            .WithErrorCode(ValidationErrorCodes.InvalidForeignKey)
            .WithMessage("Group does not exist");

        RuleFor(g => g.StudentId)
            .IsIdOfValidUserInRole(userManager, Roles.Student);
    }
}
