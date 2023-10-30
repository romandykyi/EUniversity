namespace EUniversity.Core.Dtos.Users;

public record CreatedUserDto(string Id, string UserName, string Password, bool Success, IEnumerable<string>? Errors);
