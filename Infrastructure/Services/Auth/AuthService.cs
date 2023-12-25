using EUniversity.Core.Dtos.Auth;
using EUniversity.Core.Models;
using EUniversity.Core.Services.Auth;
using Microsoft.AspNetCore.Identity;
using System.Security.Cryptography;

namespace EUniversity.Infrastructure.Services.Auth;

/// <inheritdoc cref="IAuthService" />
public class AuthService : IAuthService
{
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IAuthHelper _authHelper;

    public AuthService(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        IAuthHelper authHelper)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _authHelper = authHelper;
    }

    /// <inheritdoc />
    public async Task<RegisterResult> RegisterAsync(RegisterDto register,
        string? userName = null, string? password = null, params string[] roles)
    {
        // Generate username and password if needed
        using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
        {
            userName ??= await _authHelper.GenerateUserNameAsync(rng, register.FirstName, register.LastName);
            password ??= _authHelper.GeneratePassword(rng);
        }

        var user = new ApplicationUser()
        {
            FirstName = register.FirstName,
            LastName = register.LastName,
            MiddleName = string.IsNullOrWhiteSpace(register.MiddleName) ? null : register.MiddleName,
            Email = register.Email,
            UserName = userName
        };

        var result = await _userManager.CreateAsync(user, password);
        if (!result.Succeeded)
        {
            return new(result);
        }

        var roleResult = await _userManager.AddToRolesAsync(user, roles);
        return new(roleResult, user.Id, userName, password);
    }

    /// <inheritdoc />
    public async IAsyncEnumerable<RegisterResult> RegisterManyAsync(
        IEnumerable<RegisterDto> users, params string[] roles)
    {
        foreach (var user in users)
        {
            var result = await RegisterAsync(user, roles: roles);
            yield return result;
        }
    }

    /// <inheritdoc />
    public async Task<bool> LogInAsync(LogInDto login)
    {
        ApplicationUser? user = await _userManager.FindByNameAsync(login.UserName);
        // User is not found or deleted - log in failed
        if (user == null || user.IsDeleted)
        {
            return false;
        }
        // This doesn't count login failures towards account lockout and two factor authorization
        var result = await _signInManager.PasswordSignInAsync(
            user, login.Password, login.RememberMe, lockoutOnFailure: false);
        return result.Succeeded;
    }

    /// <inheritdoc />
    public async Task<IdentityResult> ChangePasswordAsync(string userId, ChangePasswordDto password)
    {
        var appUser = await _userManager.FindByIdAsync(userId);
        return await _userManager.ChangePasswordAsync(appUser!, password.Current, password.New);
    }
}
