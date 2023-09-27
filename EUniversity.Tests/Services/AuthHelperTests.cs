using EUniversity.Core.Models;
using EUniversity.Infrastructure.Services;
using Microsoft.AspNetCore.Identity;
using System.Security.Cryptography;

namespace EUniversity.Tests.Services
{
	public class AuthHelperTests
	{
		private RandomNumberGenerator _rngMock;
		private UserManager<ApplicationUser> _userManagerMock;

		[SetUp]
		public void SetUp()
		{
			_rngMock = Substitute.For<RandomNumberGenerator>();

			// GetBytes will return only zeros
			_rngMock.When(x => x.GetBytes(Arg.Any<byte[]>()))
				.Do(x =>
				{
					var bytes = x.ArgAt<byte[]>(0);
					for (int i = 0; i < bytes.Length; i++)
					{
						bytes[i] = 0x00;
					}
				});

			// Mock user manager
			var userStore = Substitute.For<IUserStore<ApplicationUser>>();
			_userManagerMock = Substitute.For<UserManager<ApplicationUser>>(
				userStore, null, null, null, null, null, null, null, null
				);
		}

		[TearDown]
		public void TearDown()
		{
			_rngMock.Dispose();
		}

		[Test]
		public void GeneratePassword_Always_HasAppropriateLength()
		{
			// Arrange
			AuthHelper authHelper = new(_userManagerMock);

			// Act 
			string password = authHelper.GeneratePassword(_rngMock);

			// Assert
			Assert.That(password, Has.Length.EqualTo(AuthHelper.PasswordLength));
		}

		[Test]
		public void GeneratePassword_Always_IsStrong()
		{
			// Arrange
			AuthHelper authHelper = new(_userManagerMock);

			// Act 
			string password = authHelper.GeneratePassword(_rngMock);

			// Assert
			Assert.Multiple(() =>
			{
				// Lowercase letter
				Assert.That(password.Any(char.IsAsciiLetterLower));
				// Uppercase letter
				Assert.That(password.Any(char.IsAsciiLetterUpper));
				// Number
				Assert.That(password.Any(char.IsDigit));
				// Nonalphanumerical
				Assert.That(password.Any(c => char.IsAscii(c) && !char.IsAsciiLetterOrDigit(c)));
			});
		}

		[Test]
		public async Task GenerateUserName_Always_IsUnique()
		{
			// Arrange
			int attempt = 0;
			const int attempts = 5;
			// Simulate multiple tries to get unique name
			_userManagerMock.FindByNameAsync(Arg.Any<string>())
				.Returns(x => ++attempt >= attempts ? null : new());

			AuthHelper authHelper = new(_userManagerMock);

			// Act 
			string userName = await authHelper.GenerateUserNameAsync(_rngMock, "Joe", "Doe");

			// Assert
			await _userManagerMock.Received(attempts).FindByNameAsync(Arg.Any<string>());
		}

		[Test]
		public async Task GenerateUserName_Always_ContainsAlphanumericsOnly()
		{
			// Arrange
			_userManagerMock.FindByNameAsync(Arg.Any<string>())
				.Returns<ApplicationUser?>(x => null);
			AuthHelper authHelper = new(_userManagerMock);

			// Act 
			string userName = await authHelper.GenerateUserNameAsync(_rngMock, "Joe", "Doe");

			// Assert
			Assert.That(userName.All(char.IsAsciiLetterOrDigit));
		}
	}
}
