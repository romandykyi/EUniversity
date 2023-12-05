using EUniversity.Core.Dtos.University;
using EUniversity.Core.Models;
using EUniversity.Core.Services;
using Microsoft.AspNetCore.Identity;

namespace EUniversity.Core.Validation.University;

public class ClassUpdateDtoValidator : ClassWriteDtoValidator<ClassUpdateDto>
{
    public ClassUpdateDtoValidator(IEntityExistenceChecker existenceChecker, UserManager<ApplicationUser> userManager) : base(existenceChecker, userManager)
    {
    }
}
