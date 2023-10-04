using EUniversity.Core.Dtos.Auth;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace EUniversity.Core.Dtos.Users
{
    [ValidateNever] // Remove data annotations validations
    public record RegisterUsersDto(IEnumerable<RegisterDto> Users);
}
