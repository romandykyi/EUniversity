namespace EUniversity.Core.Dtos.Auth
{
	public class LogInDto
	{
		public string UserName { get; set; } = null!;
		public string Password { get; set; } = null!;
		public bool RememberMe { get; set; }
	}
}
