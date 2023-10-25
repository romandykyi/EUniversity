using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace EUniversity.Core.Dtos.University
{
    [ValidateNever] // Remove data annotations validation
    public record CourseCreateDto(string Name, string? Description);
}
