using Duende.IdentityServer.Extensions;
using EUniversity.Core.Dtos.Auth;
using EUniversity.Core.Models;
using EUniversity.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
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
		private readonly UserManager<ApplicationUser> _userManager;

		public AuthController(IAuthService authService, UserManager<ApplicationUser> userManager)
		{
			_authService = authService;
			_userManager = userManager;
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
		public async Task<IStatusCodeActionResult> LogInAsync([FromBody] LogInDto login)
		{
			if (await _authService.LogInAsync(login))
			{
				return NoContent();
			}

			return Problem(statusCode: StatusCodes.Status401Unauthorized, title: "Invalid login attempt");
		}

		/// <summary>
		/// Changes a password of current user.
		/// </summary>
		/// <response code="204">Success</response>
		/// <response code="400">Malformed input</response>
		/// <response code="401">Unauthorized user call/Incorrect current password</response>
		[HttpPost]
		[Authorize]
		[Route("password/change")]
		[ProducesResponseType(StatusCodes.Status204NoContent)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status401Unauthorized)]
		public async Task<IStatusCodeActionResult> ChangePasswordAsync([FromBody] ChangePasswordDto password)
		{
			var result = await _authService.ChangePasswordAsync(User.GetSubjectId()!, password);
			if (!result.Succeeded)
			{
				// If current password is incorrect
				if (result.Errors.Any(e => e.Code == "PasswordMismatch"))
				{
					return Problem(statusCode: StatusCodes.Status401Unauthorized, title: "Incorrect password");
				}
				// Password validation errors
				foreach (var error in result.Errors)
				{
					ModelState.AddModelError("New", error.Description);
				}
				return BadRequest(new ValidationProblemDetails(ModelState));
			}

			return NoContent();
		}
	}
}
