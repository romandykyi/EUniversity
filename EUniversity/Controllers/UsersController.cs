using EUniversity.Core.Dtos.Users;
using EUniversity.Core.Pagination;
using EUniversity.Core.Policy;
using EUniversity.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
        [ProducesResponseType(typeof(IEnumerable<UserViewDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> GetAllUsersAsync([FromQuery] PaginationProperties paginationProperties)
        {
            return Ok(await _usersService.GetAllUsersAsync(paginationProperties));
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
        public async Task<IActionResult> GetAllStudentsAsync([FromQuery] PaginationProperties paginationProperties)
        {
            return Ok(await _usersService.GetUsersInRoleAsync(Roles.Student, paginationProperties));
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
        public async Task<IActionResult> GetAllTeachersAsync([FromQuery] PaginationProperties paginationProperties)
        {
            return Ok(await _usersService.GetUsersInRoleAsync(Roles.Teacher, paginationProperties));
        }
        #endregion

        #region Post
        private async Task<IActionResult> RegisterAsync(RegisterUsersDto students, string role, string location)
        {
            List<CreatedUserDto> createdUsers = new();
            await foreach (var result in _authService.RegisterManyAsync(students.Users, role))
            {
                CreatedUserDto createdUser = new(
                    result.UserName,
                    result.Password,
                    result.Succeeded,
                    result.Succeeded ? null : result.Errors.Select(e => e.Description)
                );
                createdUsers.Add(createdUser);
            }

            return Created(location, createdUsers);
        }

        /// <summary>
        /// Registers students.
        /// </summary>
        /// <response code="201">Returns status of students</response>
        /// <response code="400">Invalid input</response>
        /// <response code="401">Unauthorized user call</response>
        /// <response code="403">User lacks 'Administrator' role</response>
        [HttpPost]
        [Route("students")]
        [ProducesResponseType(typeof(IEnumerable<CreatedUserDto>), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> RegisterStudentsAsync([FromBody] RegisterUsersDto students)
        {
            return await RegisterAsync(students, Roles.Student, "api/users/students");
        }

        /// <summary>
        /// Registers teachers.
        /// </summary>
        /// <response code="201">Returns status of teachers</response>
        /// <response code="400">Invalid input</response>
        /// <response code="401">Unauthorized user call</response>
        /// <response code="403">User lacks 'Administrator' role</response>
        [HttpPost]
        [Route("teachers")]
        [ProducesResponseType(typeof(IEnumerable<CreatedUserDto>), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> RegisterTeachersAsync([FromBody] RegisterUsersDto teachers)
        {
            return await RegisterAsync(teachers, Roles.Teacher, "api/users/teachers");
        }
        #endregion
    }
}
