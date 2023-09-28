using EUniversity.Core.Dtos.Auth;
using EUniversity.Core.Models;
using EUniversity.Core.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace EUniversity.IntegrationTests.Services
{
	// Here we're testing only Register method, because others methods
	// are tested indirectly inside AuthControllerTests
	public class AuthServiceRegisterTests : IntegrationTest
	{
		private RegisterDto _registerDto;
		private IAuthService _authService;
		private SignInManager<ApplicationUser> _signInManager;

		[SetUp]
		public async Task SetUp()
		{
			_registerDto = new()
			{
				Email = "example@email.com",
				FirstName = "Joe",
				LastName = "Doe"
			};
			_authService = ServiceScope.ServiceProvider.GetService<IAuthService>()!;
			_signInManager = ServiceScope.ServiceProvider.GetService<SignInManager<ApplicationUser>>()!;

			await ClearEmailAsync(_registerDto.Email);
		}

		[Test]
		public async Task Register_CustomUserNameAndPassword_RegistersUser()
		{
			// Arrange
			const string userName = "user";
			const string password = "Password1!";
			await ClearUserNameAsync(userName);

			// Act
			var result = await _authService.RegisterAsync(_registerDto, userName, password);

			// Assert
			Assert.Multiple(() =>
			{
				Assert.That(result.Succeeded);
				Assert.That(UserManager.FindByNameAsync(userName), Is.Not.Null);
			});
		}

		[Test]
		public async Task Register_NoUserName_ReturnsValidUsername()
		{
			// Arrange
			const string password = "Password1!";

			// Act
			var result = await _authService.RegisterAsync(_registerDto, password: password);
			Assert.That(result.Succeeded);
			var user = await UserManager.FindByNameAsync(result.UserName);

			// Assert
			Assert.That(user, Is.Not.Null);
		}

		[Test]
		public async Task Register_NoPassword_ReturnsValidPassword()
		{
			// Arrange
			const string userName = "user";
			await ClearUserNameAsync(userName);

			// Act
			var result = await _authService.RegisterAsync(_registerDto, userName: userName);
			Assert.That(result.Succeeded);
			var user = await UserManager.FindByNameAsync(userName);
			Assert.That(user, Is.Not.Null);
			var signInResult = await _signInManager.CheckPasswordSignInAsync(user, result.Password, false);

			// Assert
			Assert.That(signInResult.Succeeded);
		}
	}
}
