using EUniversity.Core.Dtos.Auth;
using System.Net;

namespace EUniversity.IntegrationTests.Controllers
{
	public sealed class AuthControllerTests : IDisposable
	{
		private readonly ProgramWebApplicationFactory _factory;
		private readonly HttpClient _client;

		public AuthControllerTests()
		{
			_factory = new ProgramWebApplicationFactory();
			_client = _factory.CreateClient();
		}

		public void Dispose()
		{
			_client.Dispose();
			_factory.Dispose();
		}

		[Test]
		public async Task LogIn_ValidLogin_ReturnsNoContent()
		{
			// Arrange
			var validLoginDto = new LogInDto
			{
				UserName = "admin",
				Password = "Chang3M3InProduct10nPlz!"
			};

			// Act
			var response = await _client.PostAsJsonAsync("/api/auth/login", validLoginDto);

			// Assert
			Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NoContent));
		}

		[Test]
		public async Task LogIn_InvalidLogin_ReturnsUnauthorized()
		{
			// Arrange
			var invalidLoginDto = new LogInDto
			{
				UserName = "user",
				Password = "1"
			};

			// Act
			var response = await _client.PostAsJsonAsync("/api/auth/login", invalidLoginDto);

			// Assert
			Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
		}

		[Test]
		public async Task LogIn_MalformedData_ReturnsBadRequest()
		{
			// Arrange
			var malformedLoginDto = new LogInDto();

			// Act
			var response = await _client.PostAsJsonAsync("/api/auth/login", malformedLoginDto);

			// Assert
			Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
		}
	}
}
