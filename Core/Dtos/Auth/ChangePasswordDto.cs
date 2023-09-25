namespace EUniversity.Core.Dtos.Auth
{
	public class ChangePasswordDto
	{
		public string Old { get; set; } = null!;
		public string New { get; set; } = null!;
	}
}
