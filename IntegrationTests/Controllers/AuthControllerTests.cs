using EUniversity.Core.Dtos.Auth;
using Microsoft.AspNetCore.Identity;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using System.Net;

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
            LogInDto loginDto = new(DefaultUserName, DefaultPassword, false);
            WebApplicationFactory.AuthServiceMock
                .LogInAsync(loginDto).Returns(true);

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
            LogInDto loginDto = new(DefaultUserName, DefaultPassword, false);
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
            var loginDto = new LogInDto(string.Empty, string.Empty, false);
            WebApplicationFactory.AuthServiceMock
                .LogInAsync(loginDto).Throws<InvalidOperationException>();

            // Act
            var response = await client.PostAsJsonAsync("/api/auth/login", loginDto);

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }

        [Test]
        public async Task ChangePassword_ValidAttempt_Succeeds()
        {
            // Arrange
            string userId = Guid.NewGuid().ToString();
            using var client = CreateStudentClient(userId);
            ChangePasswordDto password = new(DefaultPassword, NewPassword);
            WebApplicationFactory.AuthServiceMock
                .ChangePasswordAsync(Arg.Any<string>(), Arg.Any<ChangePasswordDto>())
                .Returns(IdentityResult.Success);

            // Act
            var response = await client.PostAsJsonAsync("/api/auth/password/change", password);

            // Assert
            response.EnsureSuccessStatusCode();
            await WebApplicationFactory.AuthServiceMock
                .Received(1)
                .ChangePasswordAsync(userId, password);
        }

        [Test]
        public async Task ChangePassword_UnauthenticatedUser_Fails()
        {
            // Arrange
            using var client = CreateUnauthorizedClient();
            var password = new ChangePasswordDto(DefaultPassword, NewPassword);
            WebApplicationFactory.AuthServiceMock
                .ChangePasswordAsync(Arg.Any<string>(), Arg.Any<ChangePasswordDto>())
                .Throws<InvalidOperationException>();

            // Act
            var response = await client.PostAsJsonAsync("/api/auth/password/change", password);

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
        }

        [Test]
        public async Task ChangePassword_InvalidInput_Returns400BadRequest()
        {
            // Arrange
            using var client = CreateStudentClient();
            // Equal passwords shouldn't be allowed
            var password = new ChangePasswordDto(DefaultPassword, DefaultPassword);
            WebApplicationFactory.AuthServiceMock
                .ChangePasswordAsync(Arg.Any<string>(), Arg.Any<ChangePasswordDto>())
                .Throws<InvalidOperationException>();

            // Act
            var response = await client.PostAsJsonAsync("/api/auth/password/change", password);

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }
    }
}
