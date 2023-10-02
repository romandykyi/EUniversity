using EUniversity.Core.Dtos.Auth;
using EUniversity.Core.Models;
using EUniversity.Infrastructure.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace EUniversity.Tests.Services
{
	public class AuthServiceUnitTests
	{
		private UserManager<ApplicationUser> _userManagerMock;
		private SignInManager<ApplicationUser> _signInManagerMock;
		private AuthService _authService;

		private const string DefaultUserName = "user";
		private const string DefaultPassword = "Password1!@Gs";

		[SetUp]
		public void SetUp()
		{
			var userStore = Substitute.For<IUserStore<ApplicationUser>>();
			_userManagerMock = Substitute.For<UserManager<ApplicationUser>>(
				userStore, null, null, null, null, null, null, null, null
				);

			var contextAccessor = Substitute.For<IHttpContextAccessor>();
			var claimsFactory = Substitute.For<IUserClaimsPrincipalFactory<ApplicationUser>>();

			_signInManagerMock = Substitute.For<SignInManager<ApplicationUser>>(
				_userManagerMock, contextAccessor, claimsFactory, null, null, null, null
				);

			AuthHelper helper = new(_userManagerMock);

			_authService = new(_userManagerMock, _signInManagerMock, helper);
		}

		[Test]
		public async Task LogIn_ValidLogin_Succeeds()
		{
			// Arrange
			LogInDto login = new(DefaultUserName, DefaultPassword, true);
			_signInManagerMock
				.PasswordSignInAsync(login.UserName, login.Password, login.RememberMe, Arg.Any<bool>())
				.Returns(SignInResult.Success);

			// Act
			bool result = await _authService.LogInAsync(login);

			// Assert
			await _signInManagerMock
				.Received(1)
				.PasswordSignInAsync(login.UserName, login.Password, login.RememberMe, Arg.Any<bool>());
			Assert.That(result);
		}

		[Test]
		public async Task LogIn_InvalidLogin_Fails()
		{
			// Arrange
			LogInDto login = new(DefaultUserName, DefaultPassword, true);
			_signInManagerMock
				.PasswordSignInAsync(login.UserName, login.Password, login.RememberMe, Arg.Any<bool>())
				.Returns(SignInResult.Failed);

			// Act
			bool result = await _authService.LogInAsync(login);

			// Assert
			await _signInManagerMock
				.Received(1)
				.PasswordSignInAsync(login.UserName, login.Password, login.RememberMe, Arg.Any<bool>());
			Assert.That(result, Is.False);
		}
	}
}
