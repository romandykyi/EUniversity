using EUniversity.Core.Models;
using EUniversity.Core.Dtos.Auth;
using Microsoft.AspNetCore.Identity;

namespace EUniversity.Core.Services
{
	/// <summary>
	/// Authentication service.
	/// </summary>
	public interface IAuthService
	{
		/// <summary>
		/// Registers a user.
		/// </summary>
		/// <param name="register">Data needed for register.</param>
		/// <returns>
		/// Result of the operation.
		/// </returns>
		Task<IdentityResult> RegisterAsync(RegisterDto register);

		/// <summary>
		/// Logs in a user.
		/// </summary>
		/// <param name="login">Data needed for login.</param>
		/// <returns>
		/// <see langword="true" /> on success;
		/// otherwise <see langword="false" />.
		/// </returns>
		Task<bool> LogInAsync(LogInDto login);

		/// <summary>
		/// Logs out current user.
		/// </summary>
		Task LogOutAsync();

		/// <summary>
		/// Changes a password for the user.
		/// </summary>
		/// <param name="user">
		/// User which password will be changed.
		/// </param>
		/// <param name="password">
		/// Old and new passwords.
		/// </param>
		/// <returns>
		/// Result of the operation.
		/// </returns>
		Task<IdentityResult> ChangePasswordAsync(ApplicationUser user, ChangePasswordDto password);
	}
}
