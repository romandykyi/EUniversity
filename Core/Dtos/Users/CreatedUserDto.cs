namespace EUniversity.Core.Dtos.Users
{
    public record CreatedUserDto(string UserName, string Password, bool Success, IEnumerable<string>? Errors);
}
