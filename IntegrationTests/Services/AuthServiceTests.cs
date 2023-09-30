using EUniversity.Core.Dtos.Auth;
using EUniversity.Core.Models;
using EUniversity.Core.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace EUniversity.IntegrationTests.Services
{
	public class AuthServiceTests : ServicesTest
	{
		private SignInManager<ApplicationUser> _signInManager;
		private UserManager<ApplicationUser> _userManager;
		private IAuthService _authService;

		// Ensures that username is free.
		private async Task ClearUserNameAsync(string userName)
		{
			var user = await _userManager.FindByNameAsync(userName);
			if (user != null)
			{
				await _userManager.DeleteAsync(user);
			}
		}

		// Ensures that email is free.
		private async Task ClearEmailAsync(string email)
		{
			var user = await _userManager.FindByEmailAsync(email);
			if (user != null)
			{
				await _userManager.DeleteAsync(user);
			}
		}

		private async Task<RegisterDto> GetDefaultRegisterDtoAsync()
		{
			RegisterDto result = new()
			{
				Email = "example@email.com",
				FirstName = "Joe",
				LastName = "Doe"
			};
			await ClearEmailAsync(result.Email);
			return result;
		}

		[SetUp]
		public void SetUp()
		{
			_signInManager = ServiceScope.ServiceProvider.GetService<SignInManager<ApplicationUser>>()!;
			_userManager = ServiceScope.ServiceProvider.GetService<UserManager<ApplicationUser>>()!;
			_authService = ServiceScope.ServiceProvider.GetService<IAuthService>()!;
		}

		[Test]
		public async Task Register_CustomUserNameAndPassword_RegistersUser()
		{
			// Arrange
			const string userName = "user";
			const string password = "Password1!";
			await ClearUserNameAsync(userName);
			var registerDto = await GetDefaultRegisterDtoAsync();

			// Act
			var result = await _authService.RegisterAsync(registerDto, userName, password);

			// Assert
			Assert.Multiple(() =>
			{
				Assert.That(result.Succeeded);
				Assert.That(_userManager.FindByNameAsync(userName), Is.Not.Null);
			});
		}

		[Test]
		public async Task Register_NoUserName_ReturnsValidUsername()
		{
			// Arrange
			const string password = "Password1!";
			var registerDto = await GetDefaultRegisterDtoAsync();

			// Act
			var result = await _authService.RegisterAsync(registerDto, password: password);

			// Assert
			Assert.Multiple(async () =>
			{
				Assert.That(result.Succeeded);
				Assert.That(await _userManager.FindByNameAsync(result.UserName), Is.Not.Null);
			});
		}

		[Test]
		public async Task Register_NoPassword_ReturnsValidPassword()
		{
			// Arrange
			const string userName = "user";
			await ClearUserNameAsync(userName);
			var registerDto = await GetDefaultRegisterDtoAsync();

			// Act
			var result = await _authService.RegisterAsync(registerDto, userName: userName);

			// Assert
			Assert.That(result.Succeeded);
			var user = await _userManager.FindByNameAsync(userName);
			Assert.That(user, Is.Not.Null);
			var signInResult = await _signInManager.CheckPasswordSignInAsync(user, result.Password, false);
			Assert.That(signInResult.Succeeded);
		}
	}
}
