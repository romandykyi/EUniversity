namespace EUniversity.Core.Dtos.Users
{
	public record CreatedUserDto(string UserName, string Password, string Email, bool Success, string? ErrorMessage);
}
