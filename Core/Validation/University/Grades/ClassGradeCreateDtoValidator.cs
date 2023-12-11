using EUniversity.Core.Dtos.University.Grades;
using EUniversity.Core.Models;
using EUniversity.Core.Models.University;
using EUniversity.Core.Services;
using FluentValidation;
using Microsoft.AspNetCore.Identity;

namespace EUniversity.Core.Validation.University.Grades;

public class ClassGradeCreateDtoValidator : AssignedGradeCreateDtoValidator<ClassGradeCreateDto>
{
    public ClassGradeCreateDtoValidator(IEntityExistenceChecker existenceChecker, UserManager<ApplicationUser> userManager) :
        base(existenceChecker, userManager)
    {
        RuleFor(g => g.ActivityTypeId!.Value)
            .MustAsync(async (id, _) =>
                await existenceChecker.ExistsAsync<ActivityType, int>(id))
            .WithErrorCode(ValidationErrorCodes.InvalidForeignKey)
            .WithMessage("Activity type does not exist")
            .When(g => g.ActivityTypeId != null);
    }
}
