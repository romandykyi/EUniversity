namespace EUniversity.Core.Dtos.Users
{
	public class UserViewDto
	{
		public string Email { get; set; } = null!;
		public string UserName { get; set; } = null!;
		public string FirstName { get; set; } = null!;
		public string? MiddleName { get; set; }
		public string LastName { get; set; } = null!;
	}
}
