using Microsoft.AspNetCore.Identity;

namespace EUniversity.Core.Models
{
	public class ApplicationUser : IdentityUser
	{
		/// <summary>
		/// Max length for first, middle and last names
		/// </summary>
		public const int MaxNameLength = 50;

		public const int MaxUserNameLength = 256;
		public const int MaxEmailLength = 256;
		public const int MaxPasswordLength = 100;

		// Attributes here are used for restricting length of names in the database,
		// not for validation:

		[StringLength(MaxNameLength)]
		public string FirstName { get; set; } = null!;
		[StringLength(MaxNameLength)]
		public string LastName { get; set; } = null!;
		[StringLength(MaxNameLength)]
		public string? MiddleName { get; set; }
	}
}
