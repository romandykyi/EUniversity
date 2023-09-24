using EUniversity.Controllers;
using EUniversity.Core.Services;
using EUniversity.Core.Dtos.Auth;
using Microsoft.AspNetCore.Http;
using EUniversity.Tests.Extensions;

namespace EUniversity.Tests.Controllers
{
	public class AuthControllerTests
	{
		[Test]
		public void LogIn()
		{
			LogInDto validLogin = new() { UserName = "real_user", Password = "valid_password" };
			LogInDto invalidLogin = new() { UserName = "unreal_user", Password = "invalid_password" };

			var authService = Substitute.For<IAuthService>();
			authService.LogInAsync(validLogin).Returns(true);
			authService.LogInAsync(invalidLogin).Returns(false);

			var controller = new AuthController(authService);

			Assert.Multiple(() =>
			{
				Assert.That(controller.LogIn(validLogin).ResponseCode(), Is.EqualTo(StatusCodes.Status204NoContent));
				Assert.That(controller.LogIn(invalidLogin).ResponseCode(), Is.EqualTo(StatusCodes.Status401Unauthorized));
			});
		}

		[Test]
		public void LogOut()
		{
			var authService = Substitute.For<IAuthService>();
			authService.LogOutAsync().Returns(Task.CompletedTask);

			var controller = new AuthController(authService);

			Assert.That(controller.LogOut().ResponseCode(), Is.EqualTo(StatusCodes.Status204NoContent));
			authService.Received(1).LogOutAsync();
		}
	}
}