using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace EUniversity.Core.Dtos.Auth
{
	[ValidateNever]
	public class LogInDto
	{
		public string UserName { get; set; } = null!;
		public string Password { get; set; } = null!;
		public bool RememberMe { get; set; }
	}
}
