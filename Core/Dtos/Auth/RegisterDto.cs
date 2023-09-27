using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace EUniversity.Core.Dtos.Auth
{
	[ValidateNever] // Remove data annotations validation
	public class RegisterDto
	{
		public string Email { get; set; } = null!;
		public string FirstName { get; set; } = null!;
		public string LastName { get; set; } = null!;
		public string? MiddleName { get; set; }
	}
}
