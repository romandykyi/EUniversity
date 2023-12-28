using EUniversity.Core.Dtos.University.Grades;
using EUniversity.Core.Models.University;
using EUniversity.Core.Models.University.Grades;
using EUniversity.Core.Services;
using FluentValidation;

namespace EUniversity.Core.Validation.University.Grades;

public class AssignedGradeUpdateDtoValidator : AbstractValidator<AssignedGradeUpdateDto>
{
    public AssignedGradeUpdateDtoValidator(IEntityExistenceChecker existenceChecker)
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

        RuleFor(g => g.ActivityTypeId!.Value)
            .MustAsync(async (id, _) =>
                await existenceChecker.ExistsAsync<ActivityType, int>(id))
            .WithErrorCode(ValidationErrorCodes.InvalidForeignKey)
            .WithMessage("Activity type does not exist")
            .When(g => g.ActivityTypeId != null);
    }
}
