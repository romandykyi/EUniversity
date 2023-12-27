using Microsoft.AspNetCore.Authorization;

namespace EUniversity.Auth;

/// <summary>
/// Requirement for <see cref="AccessOnlyOwnDataAuthorizationHandler" />.
/// </summary>
public class AccessOnlyOwnDataAuthorizationRequirement : IAuthorizationRequirement
{
    /// <summary>
    /// Gets roles that are allowed to access any data.
    /// </summary>
    public IEnumerable<string> SkipRoles { get; }

    public AccessOnlyOwnDataAuthorizationRequirement() : this(Enumerable.Empty<string>())
    {

    }

    public AccessOnlyOwnDataAuthorizationRequirement(params string[] skipRoles) :
        this(skipRoles.AsEnumerable())
    {

    }

    public AccessOnlyOwnDataAuthorizationRequirement(IEnumerable<string> skipRoles)
    {
        SkipRoles = skipRoles;
    }
}
