using EUniversity.Core.Dtos.Auth;
using EUniversity.Infrastructure.Services;
using EUniversity.IntegrationTests.Services;
using Microsoft.AspNetCore.Identity;
using NSubstitute;
using System.Net;
using System.Net.Http.Headers;

namespace EUniversity.IntegrationTests.Controllers
{
	public class AuthControllerTests : ControllersTest
	{
		private const string DefaultUserName = "user";
		private const string DefaultPassword = "Password1!@Gs";
		private const string NewPassword = DefaultPassword + "2";

		[Test]
		public async Task LogIn_ValidAttempt_Succeeds()
		{
			// Arrange
			using var client = CreateUnauthorizedClient();
			LogInDto loginDto = new()
			{
				UserName = DefaultUserName,
				Password = DefaultPassword
			};
			WebApplicationFactory.AuthServiceMock
				.LogInAsync(Arg.Any<LogInDto>()).Returns(true);

			// Act
			var response = await client.PostAsJsonAsync("/api/auth/login", loginDto);

			// Assert
			response.EnsureSuccessStatusCode();
		}

		[Test]
		public async Task LogIn_InvalidAttempt_Fails()
		{
			// Arrange
			using var client = CreateUnauthorizedClient();
			LogInDto loginDto = new()
			{
				UserName = DefaultUserName,
				Password = DefaultPassword
			};
			WebApplicationFactory.AuthServiceMock
				.LogInAsync(loginDto).Returns(false);

			// Act
			var response = await client.PostAsJsonAsync("/api/auth/login", loginDto);

			// Assert
			Assert.That(response.IsSuccessStatusCode, Is.False);
		}

		[Test]
		public async Task LogIn_InvalidInput_Returns400BadRequest()
		{
			// Arrange
			using var client = CreateUnauthorizedClient();
			var loginDto = new LogInDto()
			{
				UserName = "",
				Password = ""
			};

			// Act
			var response = await client.PostAsJsonAsync("/api/auth/login", loginDto);

			// Assert
			Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
		}
	}
}
