using Microsoft.AspNetCore.Identity;

namespace EUniversity.Core.Models;

public class ApplicationUser : IdentityUser, IEntity<string>
{
    /// <summary>
    /// Max length for first, middle and last names
    /// </summary>
    public const int MaxNameLength = 50;

    public const int MaxUserNameLength = 256;
    public const int MaxEmailLength = 256;

    public const string AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._";

    // Attributes here are used for restricting length of names in the database,
    // not for validation:

    [StringLength(MaxNameLength)]
    public string FirstName { get; set; } = null!;
    [StringLength(MaxNameLength)]
    public string LastName { get; set; } = null!;
    [StringLength(MaxNameLength)]
    public string? MiddleName { get; set; }

    /// <summary>
    /// Determines whether the user is about to be deleted.
    /// If <see cref="true"/>, then user doesn't exist for client,
    /// but can be restored.
    /// </summary>
    public bool IsDeleted { get; set; } = false;
}
