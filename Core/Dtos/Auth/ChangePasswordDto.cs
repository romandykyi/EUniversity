using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace EUniversity.Core.Dtos.Auth
{
	[ValidateNever] // Remove data annotations validation
	public class ChangePasswordDto
	{
		public string Current { get; set; } = null!;
		public string New { get; set; } = null!;
	}
}
