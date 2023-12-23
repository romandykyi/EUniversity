using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace EUniversity.Core.Dtos.Users;

[ValidateNever] // Remove data annotations validation
public record ChangeRolesDto(bool? IsStudent = null, bool? IsTeacher = null, bool? IsAdministrator = null);
