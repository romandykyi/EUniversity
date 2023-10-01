using EUniversity.Core.Dtos.Auth;
using EUniversity.Core.Dtos.Users;
using EUniversity.Core.Models;
using EUniversity.Core.Policy;
using EUniversity.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Attributes;
using System.IdentityModel.Tokens.Jwt;

namespace EUniversity.Controllers
{
	[ApiController]
	[Route("api/users")]
	[FluentValidationAutoValidation]
	[Authorize(Policies.HasAdministratorPermission)]
	public class UsersController : ControllerBase
	{
		public UserManager<ApplicationUser> _userManager;
		public IAuthService _authService;

		public UsersController(UserManager<ApplicationUser> userManager, IAuthService authService)
		{
			_userManager = userManager;
			_authService = authService;
		}

		#region Get
		/// <summary>
		/// Gets all users.
		/// </summary>
		/// <response code="200">Returns all users</response>
		/// <response code="401">Unauthorized user call</response>
		/// <response code="403">User lacks 'Administrator' role</response>
		[HttpGet]
		[ProducesResponseType(typeof(IEnumerable<UserViewDto>), StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status401Unauthorized)]
		[ProducesResponseType(StatusCodes.Status403Forbidden)]
		public async Task<IActionResult> GetAllUsersAsync()
		{
			return StatusCode(StatusCodes.Status503ServiceUnavailable);
		}

		/// <summary>
		/// Gets all students.
		/// </summary>
		/// <response code="200">Returns all students</response>
		/// <response code="401">Unauthorized user call</response>
		/// <response code="403">User lacks 'Administrator' role</response>
		[HttpGet]
		[Route("students")]
		[ProducesResponseType(typeof(IEnumerable<UserViewDto>), StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status401Unauthorized)]
		[ProducesResponseType(StatusCodes.Status403Forbidden)]
		public async Task<IActionResult> GetAllStudentsAsync()
		{
			return StatusCode(StatusCodes.Status503ServiceUnavailable);
		}

		/// <summary>
		/// Gets all teachers.
		/// </summary>
		/// <response code="200">Returns all teachers</response>
		/// <response code="401">Unauthorized user call</response>
		/// <response code="403">User lacks 'Administrator' role</response>
		[HttpGet]
		[Route("teachers")]
		[ProducesResponseType(typeof(IEnumerable<UserViewDto>), StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status401Unauthorized)]
		[ProducesResponseType(StatusCodes.Status403Forbidden)]
		public async Task<IActionResult> GetAllTeachersAsync()
		{
			return StatusCode(StatusCodes.Status503ServiceUnavailable);
		}
		#endregion

		#region Post
		/// <summary>
		/// Registers students.
		/// </summary>
		/// <response code="201">Returns registered students</response>
		/// <response code="401">Unauthorized user call</response>
		/// <response code="403">User lacks 'Administrator' role</response>
		[HttpPost]
		[Route("students")]
		[ProducesResponseType(typeof(IEnumerable<CreatedUserDto>), StatusCodes.Status201Created)]
		[ProducesResponseType(StatusCodes.Status401Unauthorized)]
		[ProducesResponseType(StatusCodes.Status403Forbidden)]
		public async Task<IActionResult> RegisterStudentsAsync([FromBody] IEnumerable<RegisterDto> students)
		{
			return StatusCode(StatusCodes.Status503ServiceUnavailable);
		}

		/// <summary>
		/// Registers teachers.
		/// </summary>
		/// <response code="201">Returns registered teachers</response>
		/// <response code="401">Unauthorized user call</response>
		/// <response code="403">User lacks 'Administrator' role</response>
		[HttpPost]
		[Route("teachers")]
		[ProducesResponseType(typeof(IEnumerable<CreatedUserDto>), StatusCodes.Status201Created)]
		[ProducesResponseType(StatusCodes.Status401Unauthorized)]
		[ProducesResponseType(StatusCodes.Status403Forbidden)]
		public async Task<IActionResult> RegisterTeachersAsync([FromBody] IEnumerable<RegisterDto> teachers)
		{
			return StatusCode(StatusCodes.Status503ServiceUnavailable);
		}
		#endregion
	}
}
