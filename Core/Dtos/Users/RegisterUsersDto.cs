using EUniversity.Core.Dtos.Auth;

namespace EUniversity.Core.Dtos.Users
{
	public class RegisterUsersDto
	{
		public IEnumerable<RegisterDto> Users { get; set; } = null!;
	}
}
