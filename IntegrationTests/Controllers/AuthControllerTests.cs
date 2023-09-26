using EUniversity.Core.Dtos.Auth;
using System.Net;

namespace EUniversity.IntegrationTests.Controllers
{
	public class AuthControllerTests : IntegrationTest
	{
		[Test]
		public async Task LogIn_ValidLogin_IsSuccessfull()
		{
			// Arrange
			var validLoginDto = new LogInDto
			{
				UserName = "admin",
				Password = "Chang3M3InProduct10nPlz!"
			};

			// Act
			var response = await Client.PostAsJsonAsync("/api/auth/login", validLoginDto);

			// Assert
			Assert.That(response.IsSuccessStatusCode);
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
			var response = await Client.PostAsJsonAsync("/api/auth/login", invalidLoginDto);

			// Assert
			Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
		}

		[Test]
		public async Task LogIn_MalformedData_ReturnsBadRequest()
		{
			// Arrange
			var malformedLoginDto = new LogInDto();

			// Act
			var response = await Client.PostAsJsonAsync("/api/auth/login", malformedLoginDto);

			// Assert
			Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
		}
	}
}
