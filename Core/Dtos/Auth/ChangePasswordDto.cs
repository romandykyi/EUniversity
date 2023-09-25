namespace EUniversity.Core.Dtos.Auth
{
	public class ChangePasswordDto
	{
		public string Current { get; set; } = null!;
		public string New { get; set; } = null!;
	}
}
