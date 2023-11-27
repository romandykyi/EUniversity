namespace EUniversity.Infrastructure.Filters;

/// <summary>
/// Represents properties for filtering users.
/// </summary>
/// <param name="FullName">Optional full name to filter by.</param>
/// <param name="UserName">Optional username to filter by.</param>
/// <param name="Email">Optional email to filter by.</param>
public record UsersFilterProperties(string? FullName = null, string? UserName = null, string? Email = null);
