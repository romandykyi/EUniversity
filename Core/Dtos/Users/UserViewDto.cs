namespace EUniversity.Core.Dtos.Users;

public class UserViewDto
{
    public string Id { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string UserName { get; set; } = null!;
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string? MiddleName { get; set; }
    public bool IsDeleted { get; set; }
    public IEnumerable<string> Roles { get; set; } = null!;
}
