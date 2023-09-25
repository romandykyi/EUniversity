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
		public async Task<IdentityResult> RegisterAsync(RegisterDto register)
		{
			throw new NotImplementedException();
			var newUser = Activator.CreateInstance<ApplicationUser>();

			await _userStore.SetUserNameAsync(newUser, register.UserName, CancellationToken.None);
			await _emailStore.SetEmailAsync(newUser, register.Email, CancellationToken.None);
			return await _userManager.CreateAsync(newUser, register.Password);
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
		public async Task<IdentityResult> ChangePasswordAsync(ApplicationUser user, ChangePasswordDto password)
		{
			return await _userManager.ChangePasswordAsync(user, password.Old, password.New);
		}
	}
}
