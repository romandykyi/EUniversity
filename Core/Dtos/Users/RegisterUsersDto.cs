using EUniversity.Core.Dtos.Auth;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace EUniversity.Core.Dtos.Users
{
	[ValidateNever]
	public class RegisterUsersDto
	{
		public IEnumerable<RegisterDto> Users { get; set; } = null!;
	}
}
