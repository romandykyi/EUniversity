using EUniversity.Core.Dtos.Auth;
using EUniversity.Core.Dtos.Users;
using EUniversity.Core.Models;
using EUniversity.Core.Policy;
using EUniversity.Core.Services;
using EUniversity.Infrastructure.Data;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Attributes;

namespace EUniversity.Controllers
{
	[ApiController]
	[Route("api/users")]
	[FluentValidationAutoValidation]
	[Authorize(Policies.HasAdministratorPermission)]
	public class UsersController : ControllerBase
	{
		private readonly IAuthService _authService;
		private readonly IUsersService _usersService;

		public UsersController(IAuthService authService, IUsersService usersService)
		{
			_authService = authService;
			_usersService = usersService;
		}

		#region Get
		/// <summary>
		/// Gets all users.
		/// </summary>
		/// <response code="200">Returns all users</response>
		/// <response code="401">Unauthorized user call</response>
		/// <response code="403">User lacks 'Administrator' role</response>
		[HttpGet]
		[ProducesResponseType(typeof(UsersViewDto), StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status401Unauthorized)]
		[ProducesResponseType(StatusCodes.Status403Forbidden)]
		public async Task<IActionResult> GetAllUsersAsync()
		{
			return Ok(await _usersService.GetAllUsersAsync());
		}

		/// <summary>
		/// Gets all students.
		/// </summary>
		/// <response code="200">Returns all students</response>
		/// <response code="401">Unauthorized user call</response>
		/// <response code="403">User lacks 'Administrator' role</response>
		[HttpGet]
		[Route("students")]
		[ProducesResponseType(typeof(UsersViewDto), StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status401Unauthorized)]
		[ProducesResponseType(StatusCodes.Status403Forbidden)]
		public async Task<IActionResult> GetAllStudentsAsync()
		{
			return Ok(await _usersService.GetUsersInRoleAsync(Roles.Student));
		}

		/// <summary>
		/// Gets all teachers.
		/// </summary>
		/// <response code="200">Returns all teachers</response>
		/// <response code="401">Unauthorized user call</response>
		/// <response code="403">User lacks 'Administrator' role</response>
		[HttpGet]
		[Route("teachers")]
		[ProducesResponseType(typeof(UsersViewDto), StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status401Unauthorized)]
		[ProducesResponseType(StatusCodes.Status403Forbidden)]
		public async Task<IActionResult> GetAllTeachersAsync()
        {
            return Ok(await _usersService.GetUsersInRoleAsync(Roles.Teacher));
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
		public async Task<IActionResult> RegisterStudentsAsync([FromBody] RegisterUsersDto students)
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
		public async Task<IActionResult> RegisterTeachersAsync([FromBody] RegisterUsersDto teachers)
		{
			return StatusCode(StatusCodes.Status503ServiceUnavailable);
		}
		#endregion
	}
}
