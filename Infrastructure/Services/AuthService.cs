using EUniversity.Core.Dtos.Auth;
using EUniversity.Core.Models;
using EUniversity.Core.Services;
using Microsoft.AspNetCore.Identity;

namespace EUniversity.Infrastructure.Services
{
	/// <inheritdoc cref="IAuthService" />
	public class AuthService : IAuthService
	{
		private readonly SignInManager<ApplicationUser> _signInManager;
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly IUserStore<ApplicationUser> _userStore;
		private readonly IUserEmailStore<ApplicationUser> _emailStore;

		public AuthService(
			UserManager<ApplicationUser> userManager,
			IUserStore<ApplicationUser> userStore,
			SignInManager<ApplicationUser> signInManager)
		{
			_userManager = userManager;
			_userStore = userStore;
			_emailStore = (IUserEmailStore<ApplicationUser>)_userStore;
			_signInManager = signInManager;
		}

		/// <inheritdoc />
		public async Task<IdentityResult> RegisterAsync(RegisterDto register,
			string? username = null, string? password = null, params string[] roles)
		{
			throw new NotImplementedException();
		}

		/// <inheritdoc />
		public async Task<bool> LogInAsync(LogInDto login)
		{
			// This doesn't count login failures towards account lockout and two factor authorization
			var result = await _signInManager.PasswordSignInAsync(
				login.UserName, login.Password, login.RememberMe, lockoutOnFailure: false);
			if (result.Succeeded)
			{
				return true;
			}
			return false;
		}

		/// <inheritdoc />
		public async Task LogOutAsync()
		{
			await _signInManager.SignOutAsync();
		}

		/// <inheritdoc />
		public async Task<IdentityResult> ChangePasswordAsync(string userId, ChangePasswordDto password)
		{
			var appUser = await _userManager.FindByIdAsync(userId);
			return await _userManager.ChangePasswordAsync(appUser!, password.Current, password.New);
		}
	}
}
