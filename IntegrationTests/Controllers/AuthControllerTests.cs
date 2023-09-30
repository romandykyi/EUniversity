using EUniversity.Core.Dtos.Auth;
using System.Net;

namespace EUniversity.IntegrationTests.Controllers
{
	public class AuthControllerTests : ControllersTest
	{
		[Test]
		public async Task LogIn_MalformedData_ReturnsBadRequest()
		{
			// Arrange
			using var client = CreateUnauthorizedClient();
			var malformedLoginDto = new LogInDto()
			{
				UserName = "",
				Password = ""
			};

			// Act
			var response = await client.PostAsJsonAsync("/api/auth/login", malformedLoginDto);

			// Assert
			Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
		}
	}
}
