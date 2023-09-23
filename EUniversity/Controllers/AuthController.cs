using EUniversity.Core.Dtos.Auth;
using EUniversity.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Attributes;

namespace EUniversity.Controllers
{
	/// <summary>
	/// Authentication controller.
	/// </summary>
	[ApiController]
	[Route("api/auth")]
	[FluentValidationAutoValidation]
	public class AuthController : ControllerBase
	{
		private readonly IAuthService _authService;

		public AuthController(IAuthService authService)
		{
			_authService = authService;
		}

		/// <summary>
		/// Logs in a user.
		/// </summary>
		/// <response code="204">Success</response>
		/// <response code="400">Malformed input</response>
		/// <response code="401">Invalid login attempt</response>
		[HttpPost]
		[AllowAnonymous]
		[Route("login")]
		[ProducesResponseType(StatusCodes.Status204NoContent)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status401Unauthorized)]
		public async Task<IActionResult> LogIn([FromBody] LogInDto login)
		{
			if (await _authService.LogInAsync(login))
			{
				return NoContent();
			}

			return Problem(statusCode: StatusCodes.Status401Unauthorized, title: "Invalid login attempt");
		}

		/// <summary>
		/// Logs out a user.
		/// </summary>
		/// <response code="204">Success</response>
		/// <response code="401">Unauthorized user call</response>
		[HttpPost]
		[Authorize]
		[Route("logout")]
		[ProducesResponseType(StatusCodes.Status204NoContent)]
		[ProducesResponseType(StatusCodes.Status401Unauthorized)]
		public async Task<IActionResult> LogOut()
		{
			await _authService.LogOut();
			return NoContent();
		}
	}
}
