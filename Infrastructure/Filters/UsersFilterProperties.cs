namespace EUniversity.Infrastructure.Filters;

/// <summary>
/// Represents a sorting mode for <see cref="UsersFilter" />.
/// </summary>
public enum UsersSortingMode
{
    /// <summary>
    /// Default sorting mode, users will not be sorted.
    /// </summary>
    Default = 0,
    /// <summary>
    /// Sort users by fullname alphabetically from a to z.
    /// </summary>
    FullName = 1,
    /// <summary>
    /// Sort users by fullname alphabetically from z to a. 
    /// </summary>
    FullNameDescending = 2,
    /// <summary>
    /// Sort users by username alphabetically from a to z.
    /// </summary>
    UserName = 3,
    /// <summary>
    /// Sort users by username alphabetically from z to a. 
    /// </summary>
    UserNameDescending = 4
}

/// <summary>
/// Represents properties for filtering users.
/// </summary>
/// <param name="FullName">Optional full name to filter by.</param>
/// <param name="UserName">Optional username to filter by.</param>
/// <param name="Email">Optional email to filter by.</param>
/// <param name="SortingMode">
/// Optional sorting mode, <see cref="UsersSortingMode.FullName"/> by default.
/// </param>
public record UsersFilterProperties(string? FullName = null, 
    string? UserName = null, string? Email = null,
    UsersSortingMode SortingMode = UsersSortingMode.FullName);
