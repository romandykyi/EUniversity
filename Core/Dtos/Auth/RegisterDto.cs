using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace EUniversity.Core.Dtos.Auth
{
    [ValidateNever] // Remove data annotations validation
    public record RegisterDto(string Email, string FirstName, string LastName, string? MiddleName = null);
}
