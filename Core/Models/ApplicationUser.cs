using Microsoft.AspNetCore.Identity;

namespace EUniversity.Core.Models
{
	public class ApplicationUser : IdentityUser
	{
		public const int MaxUserNameLength = 256;
		public const int MaxEmailLength = 256;
		public const int MaxPasswordLength = 100;
	}
}