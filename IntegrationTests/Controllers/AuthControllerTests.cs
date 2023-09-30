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
			await RegisterDefaultStudentAsync();
			var validLoginDto = new LogInDto
			{
				UserName = DefaultStudentUserName,
				Password = DefaultUsersPassword
			};

			// Act
			var response = await Client.PostAsJsonAsync("/api/auth/login", validLoginDto);

			// Assert
			response.EnsureSuccessStatusCode();
		}

		[Test]
		public async Task LogIn_InvalidLogin_ReturnsUnauthorized()
		{
			// Arrange
			await RegisterDefaultStudentAsync();
			var invalidLoginDto = new LogInDto
			{
				UserName = DefaultStudentUserName,
				Password = "*******"
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
