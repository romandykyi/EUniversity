using Microsoft.AspNetCore.Identity;

namespace EUniversity.Core.Services.Auth;

/// <summary>
/// Wrapper class for IdentityResult with username and password properties.
/// </summary>
public class RegisterResult
{
    private readonly IdentityResult _identityResult;
    private readonly string? _id, _userName, _password;

    /// <inheritdoc cref="IdentityResult.Succeeded" />
    public bool Succeeded => _identityResult.Succeeded;

    /// <inheritdoc cref="IdentityResult.Errors" />
    public IEnumerable<IdentityError> Errors => _identityResult.Errors;

    /// <summary>
    /// ID of the registered user. 
    /// <see langword="null" /> on failure.
    /// </summary>
    /// <value>
    /// <see langword="null" /> when <see cref="Succeeded" /> is <see langword="false" />.
    /// </value>
    public string Id => _id!;
    /// <summary>
    /// Username of the registered user. 
    /// </summary>
    /// <value>
    /// <see langword="null" /> when <see cref="Succeeded" /> is <see langword="false" />.
    /// </value>
    public string UserName => _userName!;
    /// <summary>
    /// Password of the registered user.
    /// <see langword="null" /> on failure.
    /// </summary>
    /// <value>
    /// <see langword="null" /> when <see cref="Succeeded" /> is <see langword="false" />.
    /// </value>
    public string Password => _password!;

    public RegisterResult(IdentityResult result,
        string? id = null, string? userName = null, string? password = null)
    {
        _identityResult = result;
        if (result.Succeeded)
        {
            _id = id;
            _userName = userName;
            _password = password;
        }
    }
}
