namespace EUniversity.Core.Dtos.Users
{
	public class CreatedUserDto
	{
		public string UserName { get; set; } = null!;
		public string Password { get; set; } = null!;
		public string Email { get; set; } = null!;

		public bool Success { get; set; }
		public string? ErrorMessage { get; set; }
	}
}
